using Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Persistance.EF
{
    public class DbContextBase : DbContext
    {
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries<IEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var version = (long)entry.Property("Version").CurrentValue;
                entry.Property("Version").CurrentValue = version++;
            }

            return base.SaveChanges();
        }

        public DbContextBase(DbContextOptions options) : base(options)
        {
        }
    }
}
