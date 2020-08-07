// <copyright file="OfficeRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

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
            return this.Query().Include(c => c.RoomItems);
        }

        public override Office Update(Office entity)
        {
            // Remuevo previo set de items del Perfil. El usuario puede haber creado, eliminado o editado existentes.
            var previousItems = this.DbContext.Room.Where(t => t.OfficeId == entity.Id);
            this.DbContext.Room.RemoveRange(previousItems);

            foreach (var item in entity.RoomItems)
            {
                this.DbContext.Room.Add(item);
            }

            return base.Update(entity);
        }
    }
}
