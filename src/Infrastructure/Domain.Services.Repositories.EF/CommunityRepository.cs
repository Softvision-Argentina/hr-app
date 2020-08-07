// <copyright file="CommunityRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class CommunityRepository : Repository<Community, DataBaseContext>
    {
        public CommunityRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Community> Query()
        {
            return base.Query();
        }
    }
}
