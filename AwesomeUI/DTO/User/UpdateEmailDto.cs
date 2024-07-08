using System.ComponentModel.DataAnnotations;

namespace AwesomeUI.DTO.User;

public class UpdateEmailDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}