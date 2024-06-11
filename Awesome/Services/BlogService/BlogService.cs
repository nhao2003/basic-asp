using Awesome.Data;
using Awesome.DTOs;
using Awesome.DTOs.Blog;
using Awesome.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Services.BlogService
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _context;

        public BlogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await _context.Blogs.ToListAsync();
        }

        public async Task<Blog?> GetBlog(Guid id)
        {
            return await _context.Blogs.FindAsync(id);
        }

        public async Task<Blog> CreateBlog(CreateBlogDTO data)
        {
            var blog = new Blog
            {
                Title = data.Title,
                Description = data.Description,
                Thumbnail = data.Thumbnail,
                Author = data.Author,
                Content = data.Content,
                CreatedAt = DateTime.Now
            };
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task<Blog?> UpdateBlog(Guid id, UpdateBlogDTO blog)
        {
            var blogToUpdate = await _context.Blogs.FindAsync(id);
            if (blogToUpdate == null)
            {
                return null;
            }
            blogToUpdate.Title = blog.Title ?? blogToUpdate.Title;
            blogToUpdate.Description = blog.Description ?? blogToUpdate.Description;
            blogToUpdate.Thumbnail = blog.Thumbnail ?? blogToUpdate.Thumbnail;
            blogToUpdate.Author = blog.Author ?? blogToUpdate.Author;
            blogToUpdate.Content = blog.Content ?? blogToUpdate.Content;
            blogToUpdate.UpdatedAt = DateTime.Now;
            _context.Blogs.Update(blogToUpdate);
            await _context.SaveChangesAsync();
            return blogToUpdate;
        }

        public async Task<Blog?> DeleteBlog(Guid id)
        {
            var blogToDelete = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blogToDelete == null)
            {
                return null;
            }
            _context.Blogs.Remove(blogToDelete);
            await _context.SaveChangesAsync();
            return blogToDelete;
        }
    }
}
