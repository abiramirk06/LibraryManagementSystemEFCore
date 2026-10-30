
using LibraryManagementSystemEFCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryManagementSystemEFCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=(localdb)\MSSQLLocalDB;Database=LibraryManagementSystemEFCoreDB;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

        public override int SaveChanges()
        {
            AddAuditLogs();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            AddAuditLogs();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditLogs()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is not AuditLog &&
                    (e.State == EntityState.Added ||
                     e.State == EntityState.Modified ||
                     e.State == EntityState.Deleted))
                .ToList();

            foreach (var entry in entries)
            {
                var action = entry.State switch
                {
                    EntityState.Added => "Created",
                    EntityState.Modified => "Updated",
                    EntityState.Deleted => "Deleted",
                    _ => "Unknown"
                };

                var entityName = entry.Metadata.ClrType.Name;

                var primaryKey = entry.Properties
                    .FirstOrDefault(p =>
                        p.Metadata.IsPrimaryKey());

                int? recordId = primaryKey?.CurrentValue is int id
                    ? id
                    : null;

                var auditLog = new AuditLog
                {
                    EntityName = entityName,
                    Action = action,
                    RecordId = recordId,
                    Details = $"{action} operation performed on {entityName}",
                    CreatedAt = DateTime.Now
                };

                AuditLogs.Add(auditLog);
            }
        }

    }
}
