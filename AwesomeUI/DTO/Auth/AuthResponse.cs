using System.Text.Json.Serialization;

namespace AwesomeUI.DTO.Auth;

public class AuthResponse(string accessToken, string? refreshToken)
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = accessToken;

    [JsonPropertyName("refreshToken")]
    public string? RefreshToken { get; set; } = refreshToken;
}

