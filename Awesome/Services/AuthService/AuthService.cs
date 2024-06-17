using Awesome.Data;
using Awesome.DTOs.Auth;
using Awesome.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Awesome.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private static readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

        private readonly IConfiguration _configuration;
        private readonly string _accessSecretKey;
        private readonly string _refreshSecretKey;
        private readonly double _tokenLifeTime;
        private readonly double _refreshTokenLifeTime;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly ApplicationDbContext _context;

        public AuthService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _accessSecretKey = _configuration["Jwt:AccessSecretKey"] ??
                               throw new ArgumentNullException("Jwt:AccessSecretKey is missing in appsettings.json");
            _refreshSecretKey = _configuration["Jwt:RefreshSecretKey"] ??
                                throw new ArgumentNullException("Jwt:RefreshSecretKey is missing in appsettings.json");
            _tokenLifeTime = double.Parse(_configuration["Jwt:TokenLifeTime"] ??
                                          throw new ArgumentNullException(
                                              "Jwt:TokenLifeTime is missing in appsettings.json"));
            _refreshTokenLifeTime = double.Parse(_configuration["Jwt:RefreshTokenLifeTime"] ??
                                                 throw new ArgumentNullException(
                                                     "Jwt:RefreshTokenLifeTime is missing in appsettings.json"));
            _issuer = _configuration["Jwt:Issuer"] ??
                      throw new ArgumentNullException("Jwt:Issuer is missing in appsettings.json");
            _audience = _configuration["Jwt:Audience"] ??
                        throw new ArgumentNullException("Jwt:Audience is missing in appsettings.json");
        }

        private string GenerateToken(string sessionId, User user, string secretKey, double expiryTime)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("SessionId", sessionId),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
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
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null || passwordHasher.VerifyHashedPassword(username, user.Password, password) ==
                PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            return GenerateTokens(user);
        }

        public async Task<AuthenticationResponse> SignUp(string username, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (existingUser != null)
            {
                throw new InvalidOperationException("User already exists");
            }

            var hashedPassword = passwordHasher.HashPassword(username, password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = hashedPassword,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GenerateTokens(user);
        }

        private AuthenticationResponse GenerateTokens(User user)
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
                RefreshToken = refreshToken
            };
            _context.Sessions.Add(session);
            _context.SaveChanges();
            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<RefreshTokenResponse> RefreshToken(string refreshToken)
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

            var session = await _context.Sessions.FindAsync(Guid.Parse(sessionId));


            if (session == null || session.RefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var user = await _context.Users.FindAsync(session.UserId);
            var newAccessToken = GenerateToken(session.Id.ToString(), user, _accessSecretKey, _tokenLifeTime);
            session.UpdatedAt = DateTime.Now;
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
            };
        }
    }
}