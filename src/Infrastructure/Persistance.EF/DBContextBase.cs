using Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using Domain.Model;

namespace Persistance.EF
{
    public class DbContextBase : DbContext
    {
        private readonly IHttpContextAccessor _context;
        private readonly string defaultUser = "Unknown user";

        public DbContextBase(DbContextOptions options, IHttpContextAccessor context) : base(options)
        {
            _context = context;
        }

        private string GetUserNameFromId(int userId)
        {
            var user = base.Set<User>().FirstOrDefault(_ => _.Id == userId);
            bool hasValidName = (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName));
            return (hasValidName && user != null) ? $"{user.FirstName} {user.LastName}" : defaultUser; 
        }

        private string GetCurrentUser()
        {
            string userIdString = _context?.HttpContext?.User?.Identity?.Name ?? string.Empty;
            int userId;
            bool idParseSuccessfully = int.TryParse(userIdString, out userId);
            return idParseSuccessfully ? GetUserNameFromId(userId) : defaultUser;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var modifiedEntities = ChangeTracker.Entries<Entity<int>>()
                .Where(e => e.State == EntityState.Modified).ToList();

            var addedEntities = ChangeTracker.Entries<Entity<int>>()
                .Where(e => e.State == EntityState.Added).ToList();

            modifiedEntities.ForEach((entry) =>
            {
                entry.Entity.LastModifiedBy = GetCurrentUser();
                entry.Entity.LastModifiedDate = DateTime.UtcNow;
                entry.Entity.Version = entry.Entity.Version++;
            });

            addedEntities.ForEach((entry) =>
            {
                entry.Entity.CreatedBy = GetCurrentUser();
                entry.Entity.CreatedDate = DateTime.UtcNow;
                entry.Entity.Version = 1;
            });

            return base.SaveChanges();
        }
    }
}
