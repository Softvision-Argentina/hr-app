using Core;
using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
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
            return Query().Include(u => u.UserDashboards).ThenInclude(d => d.Dashboard);
        }

        public User Login(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == HashUtility.GetStringSha256Hash(password));

            if (user == null)
                return null;

            return user;
        }

        public int GetUserId(string user)
        {
            var userName = _dbContext.Users.Find(user);
            var id = userName.Id;
            return id;
        }
    }
}
