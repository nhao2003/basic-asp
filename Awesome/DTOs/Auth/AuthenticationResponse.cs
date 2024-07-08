using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Auth
{
    public class AuthenticationResponse
    {
        [Required]
        public required string AccessToken { get; set; }
        [Required]
        public required string RefreshToken { get; set; }
    }
}
