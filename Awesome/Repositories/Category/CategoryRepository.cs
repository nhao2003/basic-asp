using Awesome.Data;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Repositories.Category;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Models.Entities.Category?> GetAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public DbSet<Models.Entities.Category> GetAllAsync()
    {
        return _context.Categories;
    }

    public Task AddAsync(Models.Entities.Category entity)
    {
        _context.Categories.Add(entity);
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Models.Entities.Category entity)
    {
        _context.Categories.Update(entity);
        return _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Guid id)
    {
        var entity = _context.Categories.Find(id);
        if (entity == null) throw new ArgumentNullException(nameof(id));
        _context.Categories.Remove(entity);
        return _context.SaveChangesAsync();
    }
}