using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scammy.Models;

namespace Scammy.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>  // Inherit IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }  // Your other DbSets
    }
}
