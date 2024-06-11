using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace Awesome.Authentication
{
    [AttributeUsage(AttributeTargets.All)]
    public class AuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;

            if (request.Headers.Authorization == null || request.Headers.Authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new UnauthorizedResult([], context.Request);
                return Task.FromResult(0);
            }

            var token = request.Headers.Authorization.Parameter;
            if (string.IsNullOrEmpty(token))
            {
                context.ErrorResult = new UnauthorizedResult([], context.Request);
                return Task.FromResult(0);
            }

            try
            {
                var principal = ValidateToken(token);
                if (principal == null)
                {
                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                }
                else
                {
                    context.Principal = principal;
                }
            }
            catch (Exception)
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                return Task.FromResult(0);
            }
            return Task.FromResult(0);
        }
        private ClaimsPrincipal ValidateToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678910!@#$%^&*()12345678910!@#$%^&*()"));
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = true, // Optionally validate token expiration
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            return Task.CompletedTask;
        }
    }
}
