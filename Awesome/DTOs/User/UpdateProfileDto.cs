using System.ComponentModel.DataAnnotations;

namespace Awesome.DTOs.User;

public class UpdateProfileDto
{
    [Required]
    [DataType(DataType.Text)]
    [MaxLength(50)]
    public string FullName { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public required DateOnly DateOfBirth { get; set; }
}