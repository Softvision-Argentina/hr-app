using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class InterviewRepository : Repository<Interview, DataBaseContext>
    {
        public InterviewRepository (DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }
        public override IQueryable<Interview> Query()
        {
            return base.Query();
        }

        public override IQueryable<Interview> QueryEager()
        {

            return Query();
        }
    }
}
