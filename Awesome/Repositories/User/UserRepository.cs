using Awesome.Data;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Repositories.User;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public Task<Models.Entities.User?> GetAsync(Guid id)
    {
        var user = context.Users.Find(id);
        return Task.FromResult(user);
    }

    public DbSet<Models.Entities.User> GetAllAsync()
    {
        return context.Users;
    }

    public Task AddAsync(Models.Entities.User entity)
    {
        context.Users.Add(entity);
        return context.SaveChangesAsync();
    }

    public Task UpdateAsync(Models.Entities.User entity)
    {
        context.Users.Update(entity);
        return context.SaveChangesAsync();
    }

    public Task DeleteAsync(Guid id)
    {
        var user = context.Users.Find(id);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        context.Users.Remove(user);
        return context.SaveChangesAsync();
    }

    public Task<Models.Entities.User?> GetByUsername(string username)
    {
        var user = context.Users.FirstOrDefault(x => x.Username == username);
        return Task.FromResult(user);
    }
}