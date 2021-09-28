using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using AppBlogEngine.Models;

namespace AppBlogEngine.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<AppBlogEngine.Models.Roles> Roles { get; set; }
        public DbSet<AppBlogEngine.Models.Published> Published { get; set; }
        public DbSet<AppBlogEngine.Models.Comments> Comments { get; set; }
    }
}
