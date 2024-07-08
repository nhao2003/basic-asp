using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using AwesomeUI.DTO.User;
using Microsoft.Extensions.Configuration;

namespace AwesomeUI.Services;

public class UserService(
    HttpClient httpClient,
    AuthService authService,
    IConnectivity connectivity) : BaseService(httpClient, connectivity)
{
    private String? AccessToken => authService.AccessToken;

    public async Task<UserResponseDto?> GetUserAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{GetBaseUrl()}/User");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        var response = await HttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserResponseDto>(content);
    }

    public async Task<bool> UpdateUserAsync(UpdateProfileDto updateProfileDto)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{GetBaseUrl()}/User/profile");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        var json = JsonSerializer.Serialize(updateProfileDto);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await HttpClient.SendAsync(request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UploadProfilePictureAsync(FileResult fileResult)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"{GetBaseUrl()}/User/avatar");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        // Set content type to multipart/form-data
        var fileContent = new StreamContent(await fileResult.OpenReadAsync());
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(fileResult.ContentType);
        var formData = new MultipartFormDataContent
        {
            { fileContent, "file", fileResult.FileName }
        };
        request.Content = formData;
        var response = await HttpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);
        return response.IsSuccessStatusCode;
    }
}