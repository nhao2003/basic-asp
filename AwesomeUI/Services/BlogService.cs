using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AwesomeUI.Services;

public class BlogService
{
    private readonly HttpClient _httpClient;
    private AuthService _authService;
    
    public BlogService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    private List<Blog>? _blogList;
    public async Task<List<Blog>?> GetBlogs()
    {
        if (_blogList?.Count > 0)
            return _blogList;
        
        // Add Bearer token
        var request = new HttpRequestMessage(HttpMethod.Get, "http://10.0.2.2:8000/api/Blog");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.AccessToken);
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            _blogList = await response.Content.ReadFromJsonAsync(BlogContext.Default.ListBlog);
        }

        // Offline
        // using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        // using var reader = new StreamReader(stream);
        // var contents = await reader.ReadToEndAsync();
        // _monkeyList = JsonSerializer.Deserialize(contents, MonkeyContext.Default.ListMonkey);

        return _blogList;
    }
}
