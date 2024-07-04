namespace AwesomeUI.Data.Remote;

public interface IBlogRemoteData
{
    Task<List<Blog>?> GetBlogsAsync();
    Task<Blog?> GetBlogAsync(string id);
    Task<int> SaveBlogAsync(Blog blog);
    Task<int> DeleteBlogAsync(Blog blog);
}