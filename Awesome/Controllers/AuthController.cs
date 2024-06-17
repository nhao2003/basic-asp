﻿using Awesome.DTOs.Auth;
using Awesome.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Awesome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authenticationService) : ControllerBase
    {

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] AuthenticationRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            try
            {
                var user = await authenticationService.SignIn(request.Username, request.Password);
                return Ok(user);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] AuthenticationRequest? request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            try
            {
                var user = await authenticationService.SignUp(request.Username, request.Password);
                return Ok(user);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPost("signout")]
        public IActionResult SignOut()
        {
            // Implementation for sign-out (e.g., invalidate tokens, remove sessions) can be added here
            return Ok(new { message = "Signed out successfully" });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshRequest? request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest(new { message = "Invalid request" });
            }

            try
            {
                var response = await authenticationService.RefreshToken(request.RefreshToken);
                return Ok(response);
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
