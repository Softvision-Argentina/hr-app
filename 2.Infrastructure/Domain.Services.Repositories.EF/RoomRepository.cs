using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class RoomRepository : Repository<Room, DataBaseContext>
    {
        public RoomRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Room> Query()
        {
            return base.Query();
        }

        public override IQueryable<Room> QueryEager()
        {
            return Query().Include(c => c.ReservationItems);
        }

        public override Room Update(Room entity)
        {
            //Remuevo previo set de items del Perfil. El usuario puede haber creado, eliminado o editado existentes
            var previousItems = _dbContext.Reservation.Where(t => t.RoomId == entity.Id);
            _dbContext.Reservation.RemoveRange(previousItems);

            foreach (var item in entity.ReservationItems)
            {
                _dbContext.Reservation.Add(item);
            }

            return base.Update(entity);
        }
    }
}
