using AutoMapper;
using Awesome.DTOs.Category;
using Awesome.Models.Entities;
using Awesome.Services.Category;
using Microsoft.AspNetCore.Mvc;

namespace Awesome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            var category = await categoryService.CreateAsync(request);

            var response = mapper.Map<CategoryDto>(category);

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(
            [FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {
            var categories = await categoryService.GetAllAsync(query, sortBy, sortDirection, pageNumber, pageSize);

            var enumerable = categories as Category[] ?? categories.ToArray();
            var mappedResponse = enumerable.Select(mapper.Map<CategoryDto>).ToList();
            
            return Ok(mappedResponse);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var existingCategory = await categoryService.GetById(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            var response = mapper.Map<CategoryDto>(existingCategory);

            return Ok(response);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
        {
            var category = await categoryService.UpdateAsync(id, request);

            if (category == null)
            {
                return NotFound();
            }

            var response = mapper.Map<CategoryDto>(category);

            return Ok(response);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await categoryService.DeleteAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var response = mapper.Map<CategoryDto>(category);

            return Ok(response);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCategoriesTotal()
        {
            var count = await categoryService.GetCount();

            return Ok(count);
        }
    }
}