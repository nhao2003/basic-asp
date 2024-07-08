namespace Awesome.DTOs.User;

public class UserResponseDto
{
    public required string Id { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FullName { get; init; }
    public string? Avatar { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public bool EmailVerified { get; init; }
    public bool PhoneNumberVerified { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}