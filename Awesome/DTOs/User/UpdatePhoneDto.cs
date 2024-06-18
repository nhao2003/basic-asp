using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.User;

public class UpdatePhoneDto
{
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}