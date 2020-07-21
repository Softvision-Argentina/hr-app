using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class ReaddressStatusRepository : Repository<ReaddressStatus, DataBaseContext>
    {
        public ReaddressStatusRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override bool Exist(int id)
        {
            return Query().AsNoTracking().FirstOrDefault(_ => _.Id == id) != null;
        }
    }
}
