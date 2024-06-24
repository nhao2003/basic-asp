using Awesome.Data;
using Awesome.DTOs;
using Awesome.DTOs.Blog;
using Awesome.Helper;
using Awesome.Models.Entities;
using Awesome.Services.Category;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Services.BlogService
{
    public class BlogService(ApplicationDbContext context, ICategoryService categoryService) : IBlogService
    {
        public async Task<IEnumerable<Blog>> GetBlogs(QueryObject query)
        {
            var blogs = context.Blogs.Include(
                x => x.Categories
            ).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase))
                {
                    blogs = query.IsDescending ? blogs.OrderByDescending(s => s.CreatedAt) : blogs.OrderBy(s => s.CreatedAt);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            
            return await blogs.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Blog?> GetBlog(Guid id)
        {
            return await context.Blogs.Include(
                x => x.Categories
            ).FirstOrDefaultAsync(x => x.Id == id);
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
                CreatedAt = DateTime.Now,
                Categories = new List<Models.Entities.Category>()
            };
            await using var transaction = await context.Database.BeginTransactionAsync();
            foreach (var categoryId in data.Categories)
            {
                var category = await context.Categories.FindAsync(categoryId);
                if (category != null)
                {
                    blog.Categories.Add(category);
                }
            }

            context.Blogs.Add(blog);
            await context.SaveChangesAsync();
            transaction.Commit();
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