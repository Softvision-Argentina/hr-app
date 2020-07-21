using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
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
            return Query().Include(x => x.CandidateSkills).Include(x => x.Type);
        }
    }
}
