using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.Auth;

public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; }
}