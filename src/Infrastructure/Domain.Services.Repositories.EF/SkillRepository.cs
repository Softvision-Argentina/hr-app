// <copyright file="SkillRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class SkillRepository : Repository<Skill, DataBaseContext>
    {
        public SkillRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Skill> Query()
        {
            return base.Query();
        }

        public override IQueryable<Skill> QueryEager()
        {
            return this.Query().Include(x => x.CandidateSkills).Include(x => x.Type);
        }
    }
}
