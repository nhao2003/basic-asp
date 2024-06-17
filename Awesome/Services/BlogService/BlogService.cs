using Awesome.Data;
using Awesome.DTOs;
using Awesome.DTOs.Blog;
using Awesome.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Services.BlogService
{
    public class BlogService(ApplicationDbContext context) : IBlogService
    {
        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await context.Blogs.ToListAsync();
        }

        public async Task<Blog?> GetBlog(Guid id)
        {
            return await context.Blogs.FindAsync(id);
        }

        public async Task<Blog> CreateBlog(CreateBlogDto data)
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
            context.Blogs.Add(blog);
            await context.SaveChangesAsync();
            return blog;
        }

        public async Task<Blog?> UpdateBlog(Guid id, UpdateBlogDto blog)
        {
            var blogToUpdate = await context.Blogs.FindAsync(id);
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
            context.Blogs.Update(blogToUpdate);
            await context.SaveChangesAsync();
            return blogToUpdate;
        }

        public async Task<Blog?> DeleteBlog(Guid id)
        {
            var blogToDelete = await context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blogToDelete == null)
            {
                return null;
            }
            context.Blogs.Remove(blogToDelete);
            await context.SaveChangesAsync();
            return blogToDelete;
        }
    }
}
