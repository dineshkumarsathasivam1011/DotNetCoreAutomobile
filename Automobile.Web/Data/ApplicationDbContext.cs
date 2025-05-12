using Automobile.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Automobile.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Brand> BrandAutomobile { get; set; }

    }
}
