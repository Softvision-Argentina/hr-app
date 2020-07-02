using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class ReaddressReasonRepository : Repository<ReaddressReason, DataBaseContext>
    {
        public ReaddressReasonRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<ReaddressReason> QueryEager()
        {
            return Query().Include(_ => _.Type);
        }

        public override bool Exist(int id)
        {
            return Query().AsNoTracking().FirstOrDefault(_ => _.Id == id) != null;
        }
    }
}
