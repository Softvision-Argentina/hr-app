using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class EmployeeCasualtyRepository : Repository<EmployeeCasualty, DataBaseContext>
    {
        public EmployeeCasualtyRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<EmployeeCasualty> Query()
        {
            return base.Query();
        }

        public override IQueryable<EmployeeCasualty> QueryEager()
        {
            return Query();
        }
    }
}
