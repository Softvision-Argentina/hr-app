using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class ReservationRepository : Repository<Reservation, DataBaseContext>
    {
        public ReservationRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<Reservation> Query()
        {
            return base.Query().AsNoTracking().Include(r => r.Room).Include(r => r.User);
        }

    }
}
