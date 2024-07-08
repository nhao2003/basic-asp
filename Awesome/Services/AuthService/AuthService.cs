using Awesome.Data;
using Awesome.DTOs.Auth;
using Awesome.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Awesome.Repositories;
using Awesome.Repositories.Session;
using Awesome.Repositories.User;
using Awesome.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Vonage.Video.Authentication;

namespace Awesome.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly CryptoUtils _cryptoUtils;

        private readonly string _accessSecretKey;
        private readonly string _refreshSecretKey;
        private readonly double _tokenLifeTime;
        private readonly double _refreshTokenLifeTime;
        private readonly string _issuer;

        private readonly string _audience;


        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;

        public AuthService(IConfiguration configuration, CryptoUtils cryptoUtils, IUserRepository userRepository, ISessionRepository sessionRepository)
        {
            _cryptoUtils = cryptoUtils ?? throw new ArgumentNullException(nameof(cryptoUtils));
            var configuration1 = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _accessSecretKey = configuration1["Jwt:AccessSecretKey"] ??
                               throw new ArgumentNullException("Jwt:AccessSecretKey is missing in appsettings.json");
            _refreshSecretKey = configuration1["Jwt:RefreshSecretKey"] ??
                                throw new ArgumentNullException("Jwt:RefreshSecretKey is missing in appsettings.json");
            _tokenLifeTime = double.Parse(configuration1["Jwt:TokenLifeTime"] ??
                                          throw new ArgumentNullException(
                                              "Jwt:TokenLifeTime is missing in appsettings.json"));
            _refreshTokenLifeTime = double.Parse(configuration1["Jwt:RefreshTokenLifeTime"] ??
                                                 throw new ArgumentNullException(
                                                     "Jwt:RefreshTokenLifeTime is missing in appsettings.json"));
            _issuer = configuration1["Jwt:Issuer"] ??
                      throw new ArgumentNullException("Jwt:Issuer is missing in appsettings.json");
            _audience = configuration1["Jwt:Audience"] ??
                        throw new ArgumentNullException("Jwt:Audience is missing in appsettings.json");
            
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        private string GenerateToken(string sessionId, User? user, string secretKey, double expiryTime)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("SessionId", sessionId),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(type: ClaimTypes.Role, value: user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryTime),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthenticationResponse> SignIn(string username, string password)
        {
            var user = await _userRepository.GetByUsername(username);
            if (user == null || _cryptoUtils.Verify(username, user.Password, password) ==
                PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            return GenerateTokens(user);
        }

        public async Task<AuthenticationResponse> SignUp(string username, string password)
        {
            var existingUser = await _userRepository.GetByUsername(username);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists");
            }

            var hashedPassword = _cryptoUtils.Hash(username, password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = hashedPassword,
                CreatedAt = DateTime.Now,
                Role = UserRole.Admin
            };

            await _userRepository.AddAsync(user);
            return GenerateTokens(user);
        }

        private AuthenticationResponse GenerateTokens(User? user)
        {
            var id = Guid.NewGuid();

            var accessToken = GenerateToken(id.ToString(), user, _accessSecretKey, _tokenLifeTime);
            var refreshToken = GenerateToken(id.ToString(), user, _refreshSecretKey, _refreshTokenLifeTime);
            var session = new Session
            {
                Id = id,
                User = user,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                RefreshToken = _cryptoUtils.Hash(user.Username, refreshToken)
            };
            _sessionRepository.AddAsync(session);
            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<RefreshTokenResponse> RefreshToken(string refreshToken)
        {
            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_refreshSecretKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience
                }, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var sessionId = principal.FindFirst("SessionId")?.Value;

                if (sessionId == null)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var session = await _sessionRepository.GetAsync(Guid.Parse(sessionId));


                if (session == null)
                {
                    throw new SecurityTokenException("Invalid refresh token");
                }

                var user = await _userRepository.GetAsync(session.UserId);

                if (user == null || _cryptoUtils.Verify(user.Username, session.RefreshToken, refreshToken) ==
                    PasswordVerificationResult.Failed)
                {
                    throw new SecurityTokenException("Invalid refresh token");
                }

                var newAccessToken = GenerateToken(session.Id.ToString(), user, _accessSecretKey, _tokenLifeTime);
                session.UpdatedAt = DateTime.Now;
                await _sessionRepository.UpdateAsync(session);
                return new RefreshTokenResponse
                {
                    AccessToken = newAccessToken,
                };
            }

            catch (Exception e)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }
        }

        public Task SignOut(Guid userId, Guid sessionId)
        {
            var session = _sessionRepository.GetAsync(sessionId).Result;
            if (session == null || session.UserId != userId)
            {
                throw new SecurityTokenException("Invalid session");
            }

            return _sessionRepository.DeleteAsync(sessionId);
        }
    }
}