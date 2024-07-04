using SQLite;

namespace AwesomeUI.Data.Local;

public class BlogLocalData(SQLiteAsyncConnection database) : IBlogLocalData
{
    public Task<List<Blog>?> GetBlogsAsync()
    {
        return database.Table<Blog>().ToListAsync();
    }

    public Task<Blog> GetBlogAsync(string id)
    {
        return database.Table<Blog>().Where(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveBlogAsync(Blog blog)
    {
        var existingBlog = blog.Id != null ? await GetBlogAsync(blog.Id) : null;
        if (existingBlog != null)
        {
            return await database.UpdateAsync(blog);
        }
        return await database.InsertAsync(blog);
    }

    public Task<int> DeleteBlogAsync(Blog blog)
    {
        return database.DeleteAsync(blog);
    }
}