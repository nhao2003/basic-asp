﻿using System.Text;
using AwesomeUI.DTO.Auth;

namespace AwesomeUI.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    
    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    private string? _accessToken;
    private string? _refreshToken;
    
    public string? AccessToken => _accessToken;
    public string? RefreshToken => _refreshToken;
    
    public async Task<bool> SignInAsync(AuthRequest authRequest)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://10.0.2.2:8000/api/Auth/signin");
        var json = JsonSerializer.Serialize(authRequest);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response: {body}");

            if (!response.IsSuccessStatusCode) return false;
            var responseStream = await response.Content.ReadAsStreamAsync();
            var authResponse = await JsonSerializer.DeserializeAsync<AuthResponse>(responseStream);
            _accessToken = authResponse.AccessToken;
            _refreshToken = authResponse.RefreshToken;
            Debug.WriteLine($"Access Token: {_accessToken}");
            Debug.WriteLine($"Refresh Token: {_refreshToken}");
            return true;
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Unable to sign in: {e}");
            throw;
        }
    }
}