// <copyright file="RoomRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

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
            return this.Query().Include(c => c.ReservationItems);
        }

        public override Room Update(Room entity)
        {
            // Remuevo previo set de items del Perfil. El usuario puede haber creado, eliminado o editado existentes
            var previousItems = this.DbContext.Reservation.Where(t => t.RoomId == entity.Id);
            this.DbContext.Reservation.RemoveRange(previousItems);

            foreach (var item in entity.ReservationItems)
            {
                this.DbContext.Reservation.Add(item);
            }

            return base.Update(entity);
        }
    }
}
