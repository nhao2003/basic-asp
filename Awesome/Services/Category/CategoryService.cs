using Awesome.Data;
using Awesome.DTOs.Category;
using Awesome.Repositories.Category;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Services.Category;

using Models.Entities;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<Category> CreateAsync(CreateCategoryRequestDto request)
    {
        var category = new Category()
        {
            Name = request.Name,
        };

        await categoryRepository.AddAsync(category);

        return category;
    }

    public async Task<Category?> DeleteAsync(Guid id)
    {
        var existingCategory = await categoryRepository.GetAsync(id);

        if (existingCategory is null)
        {
            return null;
        }

        await categoryRepository.DeleteAsync(id);
        return existingCategory;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(
        string? query = null,
        string? sortBy = null,
        string? sortDirection = null,
        int? pageNumber = 1,
        int? pageSize = 100)
    {
        // Query
        var categories = categoryRepository.GetAllAsync().AsQueryable();

        // Filtering
        if (string.IsNullOrWhiteSpace(query) == false)
        {
            categories = categories.Where(x => x.Name.Contains(query));
        }

        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
            {
                var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);


                categories = isAsc ? categories.OrderBy(x => x.Name) : categories.OrderByDescending(x => x.Name);
            }
        }

        var skipResults = (pageNumber - 1) * pageSize;
        categories = categories.Skip(skipResults ?? 0).Take(pageSize ?? 100);

        return await categories.ToListAsync();
    }

    public async Task<Category?> GetById(Guid id)
    {
        return await categoryRepository.GetAsync(id);
    }

    public async Task<int> GetCount()
    {
        return await categoryRepository.GetAllAsync().CountAsync();
    }

    public async Task<Category?> UpdateAsync(Guid id, UpdateCategoryRequestDto request)
    {
        var existingCategory = await categoryRepository.GetAsync(id);

        if (existingCategory == null) return null;
        existingCategory.Name = request.Name;
        await categoryRepository.UpdateAsync(existingCategory);
        return existingCategory;
    }
}