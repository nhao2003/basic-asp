namespace Awesome.DTOs.User;

public class UserReponseDto
{
    public string Id { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FullName { get; set; }
    public bool EmailVerified { get; set; }
    public bool PhoneNumberVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public UserReponseDto(Models.Entities.User user)
    {
        Id = user.Id.ToString();
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
        FullName = user.FullName;
        EmailVerified = user.EmailVerifiedAt != null;
        PhoneNumberVerified = user.PhoneNumberVerifiedAt != null;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
    }
}