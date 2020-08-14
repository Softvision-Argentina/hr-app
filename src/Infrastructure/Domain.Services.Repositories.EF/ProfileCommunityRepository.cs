namespace Domain.Services.Repositories.EF
{
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using Persistance.EF;

    public class ProfileCommunityRepository : Repository<ProfileCommunity, DataBaseContext>
    {
        public ProfileCommunityRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<ProfileCommunity> Query()
        {
            return base.Query();
        }

        public override IQueryable<ProfileCommunity> QueryEager()
        {
            return base.Query()
                .Include(r => r.Community)
                .Include(r => r.Profile);
        }
    }
}
