using System.ComponentModel.DataAnnotations;

namespace AwesomeUI.DTO.User;

public class UpdatePhoneDto
{
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}