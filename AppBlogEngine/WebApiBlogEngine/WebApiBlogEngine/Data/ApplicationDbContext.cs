using Microsoft.EntityFrameworkCore;
using WebApiBlogEngine.Models;

namespace WebApiBlogEngine.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comments> Comments { get; set; }
        public DbSet<Published> Published { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        //public DbSet<RoleUser> RoleUser { get; set; }
    }
}
