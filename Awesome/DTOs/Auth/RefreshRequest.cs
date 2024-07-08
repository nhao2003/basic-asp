using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Auth;

public class RefreshRequest
{
    [Required]
    public required string RefreshToken { get; set; }
}