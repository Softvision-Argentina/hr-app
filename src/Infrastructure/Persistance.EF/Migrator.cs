// <copyright file="Migrator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Persistance.EF
{
    using System;
    using Core.Persistance;
    using DependencyInjection.Config;
    using Microsoft.EntityFrameworkCore;

    public abstract class Migrator<TContext> : IMigrator where TContext : DbContext
    {
        private readonly TContext context;

        public Migrator(TContext context)
        {
            this.context = context;
        }

        public void Migrate(DatabaseConfigurations dbConfig)
        {
            var isDatabaseModified = false;

            if (dbConfig.RunMigrations)
            {
                this.context.Database.EnsureDeleted();
                this.context.Database.EnsureCreated();
                isDatabaseModified = true;
            }

            if (dbConfig.RunSeed)
            {
                this.SeedData(this.context);
                isDatabaseModified = true;
            }

            if (isDatabaseModified)
            {
                this.context.SaveChanges();
                this.context.Dispose();
            }
        }

        protected virtual void SeedData(TContext context)
        {
            throw new NotImplementedException();
        }
    }
}
