using Awesome.DTOs;
using Awesome.DTOs.Category;

namespace Awesome.Services.Category;

using Awesome.Models.Entities;

public interface ICategoryService
{
    Task<Category> CreateAsync(CreateCategoryRequestDto request);

    Task<IEnumerable<Category>> GetAllAsync(
        string? query = null,
        string? sortBy = null,
        string? sortDirection = null,
        int? pageNumber = 1,
        int? pageSize = 100);

    Task<Category?> GetById(Guid id);

    Task<Category?> UpdateAsync(Guid id, UpdateCategoryRequestDto request);

    Task<Category?> DeleteAsync(Guid id);

    Task<int> GetCount();
    
}