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
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(ApplicationDbContext context)
        {
            _blogService = new BlogService(context);
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var blogs = await _blogService.GetBlogs();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var blog = await _blogService.GetBlog(Guid.Parse(id));
            if (blog == null)
            {
                return NotFound();
            }
            return Ok(blog);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateBlogDTO blog)
        {
            var newBlog = await _blogService.CreateBlog(blog);
            return new CreatedResult("Get", new { id = newBlog.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] UpdateBlogDTO blog)
        {
            var updatedBlog = await _blogService.UpdateBlog(Guid.Parse(id), blog);
            if (updatedBlog == null)
            {
                return NotFound();
            }
            return Ok(updatedBlog);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var deletedBlog = await _blogService.DeleteBlog(Guid.Parse(id));
            if (deletedBlog == null)
            {
                return NotFound();
            }
            return Ok(deletedBlog);
        }
    }
}
