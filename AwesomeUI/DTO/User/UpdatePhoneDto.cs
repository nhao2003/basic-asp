using System.ComponentModel.DataAnnotations;

namespace AwesomeUI.DTO.User;

public class UpdatePhoneDto
{
    [Required]
    [Phone]
    public required string PhoneNumber { get; set; }
}