using AutoMapper;
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
    public class BlogController(IBlogService blogService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var blogs = await blogService.GetBlogs();
            return Ok(blogs.Select(mapper.Map<BlogResponseDto>));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            if (!Guid.TryParse(id, out var blogId))
            {
                return BadRequest("Invalid blog id.");
            }
            var blog = await blogService.GetBlog(blogId);
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<BlogResponseDto>(blog));
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
            if (!Guid.TryParse(id, out var blogId))
            {
                return BadRequest("Invalid blog id.");
            }
            var updatedBlog = await blogService.UpdateBlog(blogId, blog);
            if (updatedBlog == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<BlogResponseDto>(updatedBlog));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (!Guid.TryParse(id, out var blogId))
            {
                return BadRequest("Invalid blog id.");
            }
            var deletedBlog = await blogService.DeleteBlog(blogId);
            if (deletedBlog == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<BlogResponseDto>(deletedBlog));
        }
    }
}
