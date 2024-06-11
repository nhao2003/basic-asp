using Awesome.DTOs.Auth;
using Awesome.Services.AuthService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Awesome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       AuthService _authenticationService;  

        public AuthController()
        {
            _authenticationService = new AuthService();
        }
        // Sign in
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(_authenticationService.SignIn(request.Username, request.Password));
        }

        // Sign up
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] AuthenticationRequest request)
        {
            return Ok();
        }

        // Sign out
        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            return Ok();
        }

        // Refresh token
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync()
        {
            return Ok();
        }
    }
}
