using System.Security.Claims;
using AutoMapper;
using Awesome.DTOs.User;
using Awesome.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Awesome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserService userService, IMapper mapper) : ControllerBase
    {
        private Guid GetUserId()
        {
            var nameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifier == null)
            {
                throw new UnauthorizedAccessException();
            }

            return Guid.Parse(nameIdentifier.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var userId = GetUserId();
            var user = await userService.GetUserAsync(userId);
            return Ok(mapper.Map<UserResponseDto>(user));
        }

        [HttpPut("email")]
        public async Task<IActionResult> UpdateEmailAsync([FromBody] UpdateEmailDto body)
        {
            var userId = GetUserId();
            var user = await userService.UpdateUserEmailAsync(userId, body.Email);
            return Ok(mapper.Map<UserResponseDto>(user));
        }

        [HttpPut("phone-number")]
        public async Task<IActionResult> UpdatePhoneNumberAsync([FromBody] UpdatePhoneDto body)
        {
            var userId = GetUserId();
            var user = await userService.UpdateUserPhoneNumberAsync(userId, body.PhoneNumber);
            return Ok(mapper.Map<UserResponseDto>(user));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileDto updateProfileDto)
        {
            var userId = GetUserId();
            var user = await userService.UpdateUserProfileAsync(userId, updateProfileDto);
            return Ok(mapper.Map<UserResponseDto>(user));
        }

        [HttpPut("avatar")]
        public async Task<IActionResult> UpdateAvatarAsync([FromForm] IFormFile? file)
        {
            var userId = GetUserId();
            
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded");
            }
            
            // Check is file is an image
            if (!file.ContentType.StartsWith("image"))
            {
                return BadRequest("File is not an image");
            }

            var user = await userService.UpdateUserAvatarAsync(userId, file.OpenReadStream());
            return Ok(mapper.Map<UserResponseDto>(user));
        }

        [HttpPost("email/verify")]
        public async Task<IActionResult> VerifyEmailAsync([FromBody] OtpDto body)
        {
            var userId = GetUserId();
            var user = await userService.VerifyEmailAsync(userId, body.OTP);
            return Ok(mapper.Map<UserResponseDto>(user));
        }

        [HttpPost("phone-number/verify")]
        public async Task<IActionResult> VerifyPhoneNumberAsync([FromBody] OtpDto body)
        {
            var userId = GetUserId();
            var user = await userService.VerifyPhoneNumberAsync(userId, body.OTP);
            return Ok(mapper.Map<UserResponseDto>(user));
        }
    }
}