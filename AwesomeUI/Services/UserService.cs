
using System.Net.Http.Headers;
using System.Text;
using AwesomeUI.DTO.User;

namespace AwesomeUI.Services;

public class UserService(HttpClient httpClient, AuthService authService) : BaseService(httpClient)
{
    private String? AccessToken => authService.AccessToken;
    
    public async Task<UserResponseDto?> GetUserAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/User");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        
        var response = await HttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserResponseDto>(content);
    }
    
    public async Task<bool> UpdateUserAsync(UpdateProfileDto updateProfileDto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/User/profile");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        
        var json = JsonSerializer.Serialize(updateProfileDto);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await HttpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }
}