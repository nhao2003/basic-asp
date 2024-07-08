using System.Net.Http.Headers;
using System.Net.Http.Json;
using AwesomeUI.Config;
using AwesomeUI.Data.Local;
using AwesomeUI.Data.Remote;
using Microsoft.Extensions.Configuration;

namespace AwesomeUI.Services;

public class BlogService(
    IConnectivity connectivity,
    HttpClient httpClient,
    IBlogLocalData blogLocalData,
    IBlogRemoteData blogRemoteData) : BaseService(httpClient, connectivity)
{
    public async Task<List<Blog>?> GetBlogs()
    {
        List<Blog>? blogs;

        if (Connectivity.NetworkAccess == NetworkAccess.Internet)
        {
            blogs = await blogRemoteData.GetBlogsAsync();
            if (blogs == null) return blogs;
            foreach (var blog in blogs)
            {
                await blogLocalData.SaveBlogAsync(blog);
            }
        }
        else
        {
            blogs = await blogLocalData.GetBlogsAsync();
        }

        return blogs;
    }
}