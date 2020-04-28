using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class CompanyCalendarRepository : Repository<CompanyCalendar, DataBaseContext>
    {
        public CompanyCalendarRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<CompanyCalendar> Query()
        {
            return base.Query();
        }

        public override IQueryable<CompanyCalendar> QueryEager()
        {
            return Query();

        }
    }
}
