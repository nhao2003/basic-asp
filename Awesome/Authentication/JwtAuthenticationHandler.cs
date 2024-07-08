using Awesome.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;

namespace Awesome.Authentication
{
    public class JwtAuthenticationHandler : JwtBearerHandler
    {
        private readonly JwtSecurityTokenHandler _tokenHandler = new();
        private readonly ApplicationDbContext _context;

        public JwtAuthenticationHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ApplicationDbContext context)
            : base(options, logger, encoder)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
            {
                return AuthenticateResult.Fail("Authorization header not found.");
            }

            var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ") ||
                !_tokenHandler.CanReadToken(authorizationHeader["Bearer ".Length..].Trim()))
            {
                return AuthenticateResult.Fail("Invalid authorization header.");
            }

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();
            if (string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Token is empty.");
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = "12345678910!@#$%^&*()12345678910!@#$%^&*()"u8.ToArray(); // Replace with your actual secret key
                var validationParameters = GetTokenValidationParameters(key);

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (!IsValidJwtToken(validatedToken)) return AuthenticateResult.Fail("Invalid token.");
                var value = principal.FindFirst("SessionId")?.Value;
                if (value == null)
                    return AuthenticateResult.Success(new AuthenticationTicket(principal,
                        JwtBearerDefaults.AuthenticationScheme));
                var session =
                    await _context.Sessions.FindAsync(Guid.Parse(value));
                if (session == null)
                {
                    return AuthenticateResult.Fail("Invalid token.");
                }

                return AuthenticateResult.Success(new AuthenticationTicket(principal,
                    JwtBearerDefaults.AuthenticationScheme));

            }
            catch (SecurityTokenException ex)
            {
                return AuthenticateResult.Fail($"Token validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }
        }

        private static TokenValidationParameters GetTokenValidationParameters(byte[] key)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // Modify as needed
                ValidateAudience = false, // Modify as needed
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Optional: reduce the default clock skew
            };
        }

        private static bool IsValidJwtToken(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtToken &&
                   jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }
}