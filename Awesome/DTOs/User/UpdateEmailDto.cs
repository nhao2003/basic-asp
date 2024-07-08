using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.User;

public class UpdateEmailDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}