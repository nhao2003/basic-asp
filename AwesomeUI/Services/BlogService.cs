using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AwesomeUI.Services;

public class BlogService
{
    readonly HttpClient _httpClient;
    public BlogService()
    {
        this._httpClient = new HttpClient();
    }

    private List<Blog>? _blogList;
    public async Task<List<Blog>?> GetBlogs()
    {
        if (_blogList?.Count > 0)
            return _blogList;
        
        // Add Bearer token
        var request = new HttpRequestMessage(HttpMethod.Get, "http://10.0.2.2:8000/api/Blog");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTZXNzaW9uSWQiOiI0Nzc5OTBlMi1kNjcxLTQwY2YtYTM3NC00NmNmMTliNTZhNzUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImVhNDk2MTE5LWVlNDMtNDdkZi04MDQ4LTVkMjM5N2RjMjgxYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJhYmNkQGFiYy5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxODc5NTI0NSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIn0.j2_O20HCtgkbe75C52rHRqhQmYDrMO6DuM4uI9L1cGY");
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
