namespace Awesome.Services.Category;
using Awesome.Models.Entities;
public interface ICategoryService
{
    Task<Category> CreateAsync(Category category);

    Task<IEnumerable<Category>> GetAllAsync(
        string? query = null, 
        string? sortBy = null, 
        string? sortDirection = null,
        int? pageNumber = 1,
        int? pageSize = 100);

    Task<Category?> GetById(Guid id);

    Task<Category?> UpdateAsync(Category category);

    Task<Category?> DeleteAsync(Guid id);

    Task<int> GetCount();
}