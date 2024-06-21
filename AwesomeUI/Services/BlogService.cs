using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AwesomeUI.Services;

public class BlogService(HttpClient httpClient, AuthService authService) : BaseService(httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    private List<Blog>? _blogList;
    public async Task<List<Blog>?> GetBlogs()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Blog");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authService.AccessToken);
        var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            _blogList = await response.Content.ReadFromJsonAsync(BlogContext.Default.ListBlog);
        }

        return _blogList;
    }
}
