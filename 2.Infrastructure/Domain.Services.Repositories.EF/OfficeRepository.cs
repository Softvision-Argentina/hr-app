using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class OfficeRepository : Repository<Office, DataBaseContext>
    {
        public OfficeRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Office> Query()
        {
            return base.Query();
        }

        public override IQueryable<Office> QueryEager()
        {
            return Query().Include(c => c.RoomItems);
        }

        public override Office Update(Office entity)
        {
            //Remuevo previo set de items del Perfil. El usuario puede haber creado, eliminado o editado existentes.
            var previousItems = _dbContext.Room.Where(t => t.OfficeId == entity.Id);
            _dbContext.Room.RemoveRange(previousItems);

            foreach (var item in entity.RoomItems)
            {
                _dbContext.Room.Add(item);
            }

            return base.Update(entity);
        }
    }
}
