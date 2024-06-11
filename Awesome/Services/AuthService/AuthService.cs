using Awesome.DTOs.Auth;
using Awesome.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Awesome.Services.AuthService
{
    public class AuthService
    {
        private const string AccessSecretKey = "12345678910!@#$%^&*()12345678910!@#$%^&*()"; // You should store this securely
        private readonly string RefreshSecretKey = "1234567891011!@#$%^&*()1234567891011!@#$%^&*()"; // You should store this securely
        private const double AccessTokenExpiryMinutes = 30;
        private const double RefreshTokenExpiryMinutes = 30 * 24 * 60;

        private string GenerateToken(User user, string serectKey, double expiryTime)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(serectKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiryTime),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public AuthenticationResponse SignIn(string username, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = password,
                CreatedAt = DateTime.Now,
                Email = "abc@abc.com",
                FullName = "John Doe",
                PhoneNumber = "1234567890"
            };
            return GenerateTokens(user);
        }
        private AuthenticationResponse GenerateTokens(User user)
        {
            var accessToken = GenerateToken(user, AccessSecretKey, AccessTokenExpiryMinutes);
            var refreshToken = GenerateToken(user, RefreshSecretKey, RefreshTokenExpiryMinutes);
            return new AuthenticationResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            }
        }

    
}
