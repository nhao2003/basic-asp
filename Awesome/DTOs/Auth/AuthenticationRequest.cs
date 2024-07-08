using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Auth
{
    public class AuthenticationRequest
    {
        [Required]
        [EmailAddress]
        public required string Username { get; set; }
        [Required]
        [MinLength(8)]
        public required string Password { get; set; }
    }
}
