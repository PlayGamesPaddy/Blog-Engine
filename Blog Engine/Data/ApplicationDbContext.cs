using Blog_Engine.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog_Engine.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Post { get; set; }
    }
}
