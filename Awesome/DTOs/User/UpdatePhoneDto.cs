using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.User;

public class UpdatePhoneDto
{
    [Required]
    [Phone]
    public required string PhoneNumber { get; set; }
}