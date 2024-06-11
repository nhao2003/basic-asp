using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Awesome.Authentication
{
    public class JwtAuthenticationBearEvent : JwtBearerEvents
    {
        public override async Task Challenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse();

            if (context.AuthenticateFailure != null)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("{\"message\":\"" + context.AuthenticateFailure.Message + "\"}");
            }
        }
    }
}
