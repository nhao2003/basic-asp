using Awesome.Data;
using Awesome.DTOs.Category;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Services.Category;

using Models.Entities;

public class CategoryService(ApplicationDbContext dbContext) : ICategoryService
{
    public async Task<Category> CreateAsync(CreateCategoryRequestDto request)
    {
        var category = new Category()
        {
            Name = request.Name,
        };

        await dbContext.Categories.AddAsync(category);
        await dbContext.SaveChangesAsync();

        return category;
    }

    public async Task<Category?> DeleteAsync(Guid id)
    {
        var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (existingCategory is null)
        {
            return null;
        }

        dbContext.Categories.Remove(existingCategory);
        await dbContext.SaveChangesAsync();
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
        var categories = dbContext.Categories.AsQueryable();

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
        return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> GetCount()
    {
        return await dbContext.Categories.CountAsync();
    }

    public async Task<Category?> UpdateAsync(Guid id, UpdateCategoryRequestDto request)
    {
        var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (existingCategory == null) return null;
        existingCategory.Name = request.Name;
        dbContext.Entry(existingCategory).CurrentValues.SetValues(existingCategory);
        await dbContext.SaveChangesAsync();
        return existingCategory;
    }
}