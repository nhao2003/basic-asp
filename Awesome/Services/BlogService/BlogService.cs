using Awesome.Data;
using Awesome.DTOs;
using Awesome.DTOs.Blog;
using Awesome.Helper;
using Awesome.Models.Entities;
using Awesome.Repositories.Blog;
using Awesome.Services.Category;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Services.BlogService
{
    public class BlogService(IBlogRepository blogRepository, ICategoryService categoryService) : IBlogService
    {
        public async Task<IEnumerable<Blog>> GetBlogs(QueryObject query)
        {
            // var blogs = context.Blogs.Include(
            //     x => x.Categories
            // ).AsQueryable();
            var blogs = blogRepository.GetAllAsync().Include(
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
            return await blogRepository.GetAllAsync().Include(
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
            foreach (var categoryId in data.Categories)
            {
                var category = await categoryService.GetById(categoryId);
                if (category != null)
                {
                    blog.Categories.Add(category);
                }
            }

            await blogRepository.AddAsync(blog);
            return blog;
        }

        public async Task<Blog?> UpdateBlog(Guid id, UpdateBlogDto blog)
        {
            var blogToUpdate = await blogRepository.GetAsync(id);
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
            
            await blogRepository.UpdateAsync(blogToUpdate);
            return blogToUpdate;
        }

        public async Task<Blog?> DeleteBlog(Guid id)
        {
            var blogToDelete = await blogRepository.GetAsync(id);
            if (blogToDelete == null)
            {
                return null;
            }

            await blogRepository.DeleteAsync(id);
            return blogToDelete;
        }
    }
}