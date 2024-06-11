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
    }
}
