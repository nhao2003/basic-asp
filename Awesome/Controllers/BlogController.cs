using Awesome.Data;
using Awesome.DTOs;
using Awesome.DTOs.Blog;
using Awesome.Services.BlogService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Awesome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController(IBlogService blogService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var blogs = await blogService.GetBlogs();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var blog = await blogService.GetBlog(Guid.Parse(id));
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostAsync([FromBody] CreateBlogDto blog)
        {
            var newBlog = await blogService.CreateBlog(blog);
            return new CreatedResult("Get", new { id = newBlog.Id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] UpdateBlogDto blog)
        {
            var updatedBlog = await blogService.UpdateBlog(Guid.Parse(id), blog);
            if (updatedBlog == null)
            {
                return NotFound();
            }
            return Ok(updatedBlog);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var deletedBlog = await blogService.DeleteBlog(Guid.Parse(id));
            if (deletedBlog == null)
            {
                return NotFound();
            }
            return Ok(deletedBlog);
        }
    }
}
