// <copyright file="TaskRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class TaskRepository : Repository<Task, DataBaseContext>
    {
        public TaskRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Task> Query()
        {
            return base.Query();
        }

        public override IQueryable<Task> QueryEager()
        {
            return this.Query().Include(c => c.TaskItems).Include(c => c.User);
        }

        public override Task Update(Task entity)
        {
            // Remuevo previo set de items de la Task. El usuario puede haber creado, eliminado o editado existentes.
            var previousItems = this.DbContext.TaskItems.Where(t => t.TaskId == entity.Id);
            this.DbContext.TaskItems.RemoveRange(previousItems);

            foreach (var item in entity.TaskItems)
            {
                this.DbContext.TaskItems.Add(item);
            }

            return base.Update(entity);
        }
    }
}
