using Awesome.DTOs.Blog;
using Awesome.DTOs;
using Awesome.Models.Entities;
namespace Awesome.Services.BlogService
{
    public interface IBlogService
    {
        public Task<IEnumerable<Blog>> GetBlogs();
        public Task<Blog?> GetBlog(Guid id);
        public Task<Blog> CreateBlog(CreateBlogDTO blog);
        public Task<Blog?> UpdateBlog(Guid id, UpdateBlogDTO blog);
        public Task<Blog?> DeleteBlog(Guid id);
    }
}
