using Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Persistance.EF.Extensions
{
    public static class ContextExtension
    {
        public readonly static string DisableTablesConstraintsCommand = "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' ";
        public readonly static string DeleteDatabaseDataCommand = "EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'";
        public readonly static string EnableTablesConstraintsCommand = "EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
        public readonly static string ResetAllIds = "EXEC sp_MSForEachTable 'IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT(''?'', RESEED, 0)'";
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

        public static void SetupDatabaseForTesting(this DbContext context)
        {
            EntitiesSeeded = null;
            EntitySeeded = null;
            context.Database.EnsureCreated();
            context.Database.ExecuteSqlCommand(ContextExtension.DisableTablesConstraintsCommand);
            context.Database.ExecuteSqlCommand(ContextExtension.DeleteDatabaseDataCommand);
            context.Database.ExecuteSqlCommand(ContextExtension.EnableTablesConstraintsCommand);
        }

        public static void SeedDatabaseWith<T>(this DbContextBase context, IEnumerable<T> entities) where T : Entity<int>
        {
            var entityList = new List<T>();

            foreach (var entity in entities)
            {
                entity.WithPropertyValue("Id", default(int));
                entityList.Add((T) entity);
            }

            context.Set<T>().AddRange(entityList);
            context.SaveChanges();
            EntitiesSeeded = entityList;
        }

        public static void SeedDatabaseWith<T>(this DbContextBase context, T entity) where T : Entity<int>
        {
            entity.WithPropertyValue("Id", default(int));
            context.Set<T>().Add(entity);
            context.SaveChanges();
            EntitySeeded = entity;
            context.Entry(entity).State = EntityState.Detached;
        }

        public static void SeedDatabaseWithDummy<T>(this DbContextBase context, int entityCount = 1) where T : Entity<int>
        {
            var entityList = new List<T>();

            for (int i = 0; i < entityCount; i++)
            {
                var dummyEntity = DataFactory.CreateInstance<T>()
                    .WithPropertyValue("Id", default(int));

                entityList.Add(dummyEntity as T);
            }

            context.Set<T>().AddRange(entityList);
            context.SaveChanges();
            EntitiesSeeded = entityList;
        }
    }
}
