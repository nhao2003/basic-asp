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
    public class AuthService
    {
        private static readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

        private readonly IConfiguration _configuration;
        private readonly string AccessSecretKey;
        private readonly string RefreshSecretKey;
        private readonly double TokenLifeTime;
        private readonly double RefreshTokenLifeTime;
        private readonly string Issuer;
        private readonly string Audience;
        private readonly ApplicationDbContext _context;

        public AuthService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));

            AccessSecretKey = _configuration["Jwt:AccessSecretKey"] ?? throw new ArgumentNullException("Jwt:AccessSecretKey is missing in appsettings.json");
            RefreshSecretKey = _configuration["Jwt:RefreshSecretKey"] ?? throw new ArgumentNullException("Jwt:RefreshSecretKey is missing in appsettings.json");
            TokenLifeTime = double.Parse(_configuration["Jwt:TokenLifeTime"] ?? throw new ArgumentNullException("Jwt:TokenLifeTime is missing in appsettings.json"));
            RefreshTokenLifeTime = double.Parse(_configuration["Jwt:RefreshTokenLifeTime"] ?? throw new ArgumentNullException("Jwt:RefreshTokenLifeTime is missing in appsettings.json"));
            Issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing in appsettings.json");
            Audience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is missing in appsettings.json");
        }

        private string GenerateToken(User user, string secretKey, double expiryTime)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryTime),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthenticationResponse> SignIn(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null || passwordHasher.VerifyHashedPassword(username, user.Password, password) == PasswordVerificationResult.Failed)
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
            var accessToken = GenerateToken(user, AccessSecretKey, TokenLifeTime);
            var refreshToken = GenerateToken(user, RefreshSecretKey, RefreshTokenLifeTime);

            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthenticationResponse> RefreshToken(string refreshToken)
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(RefreshSecretKey)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidIssuer = Issuer,
                ValidAudience = Audience
            }, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var session = await _context.Sessions.Include(s => s.User).FirstOrDefaultAsync(x => x.User.Id.ToString() == userId);

            if (session == null || session.RefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            return GenerateTokens(session.User);
        }
    }
}
