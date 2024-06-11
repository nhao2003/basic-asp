using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Auth
{
    public class AuthenticationRequest
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
