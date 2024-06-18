using Awesome.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Awesome.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<User?> Users { get; set; }

        public DbSet<Session> Sessions { get; set; }
    }
}
