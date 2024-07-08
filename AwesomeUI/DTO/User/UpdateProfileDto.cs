using System.ComponentModel.DataAnnotations;

namespace AwesomeUI.DTO.User;

public class UpdateProfileDto
{
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(50)]
    public required string FullName { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateOnly DateOfBirth { get; set; }
}