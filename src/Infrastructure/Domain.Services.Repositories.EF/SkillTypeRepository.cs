using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class SkillTypeRepository: Repository<SkillType, DataBaseContext>
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
            return Query();
        }
    }
}
