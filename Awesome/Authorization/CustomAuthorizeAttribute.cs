using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Awesome.Authorization
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Custom authorization logic here, e.g., checking user roles or claims
            var claimsIdentity = user.Identity as System.Security.Claims.ClaimsIdentity;
            var roleClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role);
            if (roleClaim == null || roleClaim.Value != "Admin")
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
