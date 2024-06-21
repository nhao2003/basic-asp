using System.ComponentModel.DataAnnotations;

namespace AwesomeUI.DTO.User;

public class UpdateEmailDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}