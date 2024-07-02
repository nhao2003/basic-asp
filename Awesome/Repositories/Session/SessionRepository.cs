using Microsoft.EntityFrameworkCore;
using System.Linq;
using Awesome.Models.Entities;
using System.Threading.Tasks;
using Awesome.Data;

namespace Awesome.Repositories.Session
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ApplicationDbContext _context;

        public SessionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Entities.Session?> GetAsync(Guid id)
        {
            return await _context.Set<Models.Entities.Session>().FindAsync(id);
        }

        public DbSet<Models.Entities.Session> GetAllAsync()
        {
            return _context.Sessions;
        }


        public async Task AddAsync(Models.Entities.Session entity)
        {
            await _context.Set<Models.Entities.Session>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.Entities.Session entity)
        {
            _context.Set<Models.Entities.Session>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Set<Models.Entities.Session>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<Models.Entities.Session>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Models.Entities.Session?> GetByTokenAsync(string token)
        {
            return await _context.Set<Models.Entities.Session>()
                .FirstOrDefaultAsync(s => s.RefreshToken == token);
        }
    }
}