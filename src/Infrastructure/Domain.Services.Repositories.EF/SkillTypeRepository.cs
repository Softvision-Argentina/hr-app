// <copyright file="SkillTypeRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class SkillTypeRepository : Repository<SkillType, DataBaseContext>
    {
        public SkillTypeRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<SkillType> Query()
        {
            return base.Query();
        }

        public override IQueryable<SkillType> QueryEager()
        {
            return this.Query();
        }
    }
}
