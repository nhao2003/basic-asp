using Awesome.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Awesome.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetAsync(Guid id);
    DbSet<T> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}