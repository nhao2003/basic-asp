using System.Net.Http.Json;
using System.Text;
using AwesomeUI.DTO.Auth;

namespace AwesomeUI.Services;

public class AuthService(HttpClient httpClient) : BaseService(httpClient)
{
    
    
    private string? _accessToken;
    private string? _refreshToken;
    
    public string? AccessToken => _accessToken;

    public string? RefreshToken => _refreshToken;
    
    public async Task<bool> SignInAsync(AuthRequest authRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/Auth/signin");
        var json = JsonSerializer.Serialize(authRequest);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {body}");

            if (!response.IsSuccessStatusCode) return false;
            var responseStream = await response.Content.ReadAsStreamAsync();
            var authResponse = await JsonSerializer.DeserializeAsync<AuthResponse>(responseStream);
            _accessToken = authResponse.AccessToken;
            _refreshToken = authResponse.RefreshToken;
            Debug.WriteLine($"Access Token: {_accessToken}");
            Debug.WriteLine($"Refresh Token: {_refreshToken}");
            await SecureStorage.SetAsync("AccessToken", _accessToken);
            if (_refreshToken != null) await SecureStorage.SetAsync("RefreshToken", _refreshToken);
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Unable to sign in: {e}");
            throw;
        }
    }
    
    public async Task<string?> SignUpAsync(AuthRequest authRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/Auth/signup");
        var json = JsonSerializer.Serialize(authRequest);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {body}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return error?["message"] ?? "Please check your credentials and try again.";
            }
            var content = await response.Content.ReadFromJsonAsync<AuthResponse>();
            _accessToken = content.AccessToken;
            _refreshToken = content.RefreshToken;
            Debug.WriteLine($"Access Token: {_accessToken}");
            Debug.WriteLine($"Refresh Token: {_refreshToken}");
            await SecureStorage.SetAsync("AccessToken", _accessToken);
            if (_refreshToken != null) await SecureStorage.SetAsync("RefreshToken", _refreshToken);
            return null;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Unable to sign up: {e}");
            throw;
        }
    }
    
    public async Task<bool> RefreshTokenAsync()
    {
        var refreshToken = await SecureStorage.GetAsync("RefreshToken");
        if (refreshToken == null) return false;
        var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/Auth/refresh");
        var json = JsonSerializer.Serialize(new { refreshToken });
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {body}");

            if (!response.IsSuccessStatusCode) return false;
            var responseStream = await response.Content.ReadAsStreamAsync();
            var authResponse = await JsonSerializer.DeserializeAsync<AuthResponse>(responseStream);
            _accessToken = authResponse?.AccessToken;
            if (_accessToken != null) await SecureStorage.SetAsync("AccessToken", _accessToken);
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Unable to refresh token: {e}");
            throw;
        }
    }
}