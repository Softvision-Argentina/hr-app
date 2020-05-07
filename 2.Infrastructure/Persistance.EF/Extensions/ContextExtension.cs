using Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Persistance.EF.Extensions
{
    public static class ContextExtension
    {
        public static readonly string DisableTablesConstraintsCommand = "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' ";
        public static readonly string DeleteDatabaseDataCommandExceptMigrations = "EXEC sp_MSforeachtable 'if (\"?\" NOT IN (\"[dbo].[__EFMigrationsHistory]\")) DELETE FROM ?'";
        public static readonly string EnableTablesConstraintsCommand = "EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
        public static readonly string ResetAllIds = "EXEC sp_MSForEachTable 'IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT(''?'', RESEED, 0)'";
    }

    public static class TestDatabaseHelper
    {
        public static IEnumerable<Entity<int>> EntitiesSeeded;
        public static Entity<int> EntitySeeded;

        public static void ResetAllIdentitiesId(this DbContext context)
        {
            context.Database.EnsureCreated();
            context.Database.ExecuteSqlCommand(ContextExtension.ResetAllIds);
        }

        public static async Task ResetAllIdentitiesIdAsync(this DbContext context)
        {
            await context.Database.EnsureCreatedAsync();
            await context.Database.ExecuteSqlCommandAsync(ContextExtension.ResetAllIds);
        }

        public static void DeleteAllEntities(this DbContext context)
        {
            context.Database.ExecuteSqlCommand(ContextExtension.DisableTablesConstraintsCommand);
            context.Database.ExecuteSqlCommand(ContextExtension.DeleteDatabaseDataCommandExceptMigrations);
            context.Database.ExecuteSqlCommand(ContextExtension.EnableTablesConstraintsCommand);
        }

        public static void DetachAllEntities(this DbContextBase context)
        {
            var changedEntriesCopy = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
