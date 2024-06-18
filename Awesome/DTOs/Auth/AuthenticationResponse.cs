using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Auth
{
    public class AuthenticationResponse
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
