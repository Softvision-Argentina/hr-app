// <copyright file="DBContextBase.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Persistance.EF
{
    using System;
    using System.Linq;
    using Core;
    using Domain.Model;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class DbContextBase : DbContext
    {
        private readonly IHttpContextAccessor context;
        private readonly string defaultUser = "Unknown user";

        public DbContextBase(DbContextOptions options, IHttpContextAccessor context) : base(options)
        {
            this.context = context;
        }

        private string GetUserNameFromId(int userId)
        {
            var user = this.Set<User>().FirstOrDefault(_ => _.Id == userId);
            bool hasValidName = !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName);
            return (hasValidName && user != null) ? $"{user.FirstName} {user.LastName}" : this.defaultUser;
        }

        private string GetCurrentUser()
        {
            string userIdString = this.context?.HttpContext?.User?.Identity?.Name ?? string.Empty;
            int userId;
            bool idParseSuccessfully = int.TryParse(userIdString, out userId);
            return idParseSuccessfully ? this.GetUserNameFromId(userId) : this.defaultUser;
        }

        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();

            var modifiedEntities = this.ChangeTracker.Entries<Entity<int>>()
                .Where(e => e.State == EntityState.Modified).ToList();

            var addedEntities = this.ChangeTracker.Entries<Entity<int>>()
                .Where(e => e.State == EntityState.Added).ToList();

            modifiedEntities.ForEach((entry) =>
            {
                entry.Entity.LastModifiedBy = this.GetCurrentUser();
                entry.Entity.LastModifiedDate = DateTime.UtcNow;
                entry.Entity.Version = entry.Entity.Version++;
            });

            addedEntities.ForEach((entry) =>
            {
                entry.Entity.CreatedBy = this.GetCurrentUser();
                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.Version = 1;
            });

            return base.SaveChanges();
        }
    }
}
