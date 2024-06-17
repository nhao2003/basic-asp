using Awesome.DTOs.Auth;

namespace Awesome.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> SignIn(string username, string password);
        Task<AuthenticationResponse> SignUp(string username, string password);
        Task<RefreshTokenResponse> RefreshToken(string refreshToken);
    }
}