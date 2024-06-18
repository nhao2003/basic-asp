using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Auth;

public class RefreshTokenResponse
{
    [Required]
    public required string AccessToken { get; set; }
}