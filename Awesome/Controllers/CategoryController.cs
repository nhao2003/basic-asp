using Awesome.DTOs;
using Awesome.DTOs.Category;
using Awesome.Models.Entities;
using Awesome.Services.Category;
using Microsoft.AspNetCore.Mvc;

namespace Awesome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
         private readonly ICategoryService categoryRepository;

        public CategoryController(ICategoryService categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            // Map DTO to Domain Model
            var category = new Category()
            {
                Name = request.Name,
            };

            await categoryRepository.CreateAsync(category);

            // Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };

            return Ok(response);
        }

        // GET: https://localhost:7226/api/Categories?query=html&sortBy=name&sortDirection=desc
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {
            var caterogies = await categoryRepository
                .GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);

            // Map Domain model to DTO

            var response = new List<CategoryDto>();
            foreach (var category in caterogies)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                });
            }

            return Ok(response);
        }

        // GET: https://localhost:7226/api/categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);

            if (existingCategory is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
            };

            return Ok(response);
        }

        // PUT: https://localhost:7226/api/categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryRequestDto request)
        {
            // Convert DTO to Domain Model
            var category = new Category()
            {
                Id = id,
                Name = request.Name,
            };

            category = await categoryRepository.UpdateAsync(category);

            if (category == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };

            return Ok(response);
        }


        // DELETE: https://localhost:7226/api/categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryRepository.DeleteAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };

            return Ok(response);
        }


        // GET: https://localhost:7226/api/categories/count
        [HttpGet]
        [Route("count")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetCategoriesTotal()
        {
            var count = await categoryRepository.GetCount();

            return Ok(count);
        }
    }
}
