using Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Persistance.EF.Extensions
{
    public static class ContextExtension
    {
        public static string DisableTablesConstraintCommand = "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' ";
        public static string DeleteDatabaseDataCommand = "EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'";
        public static string EnableTablesConstraintCommand = "EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'";
    }

    public static class TestDatabaseHelper
    {
        public static IEnumerable<Entity<int>> EntitiesSeeded;
        public static Entity<int> EntitySeeded;

        public static void SetupDatabaseForTesting(this DbContext context)
        {
            EntitiesSeeded = null;
            EntitySeeded = null;
            context.Database.EnsureCreated();
            context.Database.ExecuteSqlCommand(ContextExtension.DisableTablesConstraintCommand);
            context.Database.ExecuteSqlCommand(ContextExtension.DeleteDatabaseDataCommand);
            context.Database.ExecuteSqlCommand(ContextExtension.EnableTablesConstraintCommand);
        }

        public static void SeedDatabaseWith<T>(this DbContextBase arLabContext, IEnumerable<T> entities) where T : Entity<int>
        {
            var entityList = new List<T>();

            foreach (var entity in entities)
            {
                entity.WithPropertyValue("Id", default(int));
                entityList.Add(entity as T);
            }

            arLabContext.Set<T>().AddRange(entityList);
            arLabContext.SaveChanges();
            EntitiesSeeded = entityList;
        }

        public static void SeedDatabaseWith<T>(this DbContextBase arLabContext, T entity) where T : Entity<int>
        {
            entity.WithPropertyValue("Id", default(int));
            arLabContext.Set<T>().Add(entity);
            arLabContext.SaveChanges();
            EntitySeeded = entity;
        }

        public static void SeedDatabaseWithDummy<T>(this DbContextBase arLabContext, int entityCount = 1) where T : Entity<int>
        {
            var entityList = new List<T>();

            for (int i = 0; i < entityCount; i++)
            {
                var dummyEntity = DataFactory.CreateInstance<T>()
                    .WithPropertyValue("Id", default(int));

                entityList.Add(dummyEntity as T);
            }

            arLabContext.Set<T>().AddRange(entityList);
            arLabContext.SaveChanges();
            EntitiesSeeded = entityList;
        }
    }
}
