namespace AwesomeUI.Data.Local;

public interface IBlogLocalData
{
    Task<List<Blog>?> GetBlogsAsync();
    Task<Blog> GetBlogAsync(string id);
    Task<int> SaveBlogAsync(Blog blog);
    Task<int> DeleteBlogAsync(Blog blog);
}