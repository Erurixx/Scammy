using Microsoft.EntityFrameworkCore;
using Scammy.Models;

namespace Scammy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }  // You can add more DbSets later
    }
}
