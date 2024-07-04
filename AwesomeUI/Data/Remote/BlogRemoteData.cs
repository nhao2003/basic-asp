using System.Net.Http.Headers;
using System.Net.Http.Json;
using AwesomeUI.Services;

namespace AwesomeUI.Data.Remote;

public class BlogRemoteData(HttpClient httpClient) : BaseRemoteData(httpClient), IBlogRemoteData
{
    private readonly AuthService _authService;
    
    public BlogRemoteData(HttpClient httpClient, AuthService authService) : this(httpClient)
    {
        this._authService = authService;
    }

    public async Task<List<Blog>?> GetBlogsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Blog");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync(BlogContext.Default.ListBlog);
        }
        return null;
    }

    public async Task<Blog?> GetBlogAsync(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Blog/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
        var response = await httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync(BlogContext.Default.Blog);
        }
        
        return null;
    }

    public Task<int> SaveBlogAsync(Blog blog)
    {
        return Task.FromResult(0);
    }

    public Task<int> DeleteBlogAsync(Blog blog)
    {
        return Task.FromResult(0);
    }
}