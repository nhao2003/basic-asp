using Microsoft.EntityFrameworkCore;
using System.Linq;
using Awesome.Models.Entities;
using System.Threading.Tasks;
using Awesome.Data;

namespace Awesome.Repositories.Blog
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Entities.Blog?> GetAsync(Guid id)
        {
            return await _context.Blogs.FindAsync(id);
        }

        public DbSet<Models.Entities.Blog> GetAllAsync()
        {
            return _context.Blogs;
        }


        public async Task AddAsync(Models.Entities.Blog entity)
        {
            await _context.Blogs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.Entities.Blog entity)
        {
            _context.Blogs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Blogs.FindAsync(id);
            if (entity != null)
            {
                _context.Blogs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}