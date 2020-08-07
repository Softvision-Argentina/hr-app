// <copyright file="CandidateProfileRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class CandidateProfileRepository : Repository<CandidateProfile, DataBaseContext>
    {
        public CandidateProfileRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<CandidateProfile> Query()
        {
            return base.Query();
        }

        public override IQueryable<CandidateProfile> QueryEager()
        {
            return this.Query().Include(c => c.CommunityItems);
        }

        public override CandidateProfile Update(CandidateProfile entity)
        {
            var previousItems = this.DbContext.Community.Where(t => t.ProfileId == entity.Id);
            this.DbContext.Community.RemoveRange(previousItems);

            foreach (var item in entity.CommunityItems)
            {
                this.DbContext.Community.Add(item);
            }

            return base.Update(entity);
        }
    }
}
