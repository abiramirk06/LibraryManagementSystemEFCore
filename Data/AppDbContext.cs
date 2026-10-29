
using LibraryManagementSystemEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystemEFCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=LibraryManagementSystemEFCoreDB;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }
    }
}
