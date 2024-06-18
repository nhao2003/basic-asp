using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.User;

public class UpdateEmailDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}