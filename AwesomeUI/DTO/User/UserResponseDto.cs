using System.Text.Json.Serialization;

namespace AwesomeUI.DTO.User;

public class UserResponseDto
{
    public string Id { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("phoneNumber")]
    public string? PhoneNumber { get; set; }
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
    [JsonPropertyName("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }
    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }
    [JsonPropertyName("phoneNumberVerified")]
    public bool PhoneNumberVerified { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}