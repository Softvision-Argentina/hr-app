// <copyright file="UserRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class UserRepository : Repository<User, DataBaseContext>
    {
        public UserRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<User> Query()
        {
            return base.Query();
        }

        public override IQueryable<User> QueryEager()
        {
            return this.Query().Include(u => u.UserDashboards).ThenInclude(d => d.Dashboard);
        }

        public User Login(string username, string password)
        {
            var user = this.DbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == HashUtility.GetStringSha256Hash(password));

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public int GetUserId(string user)
        {
            var userName = this.DbContext.Users.Find(user);
            var id = userName.Id;
            return id;
        }
    }
}
