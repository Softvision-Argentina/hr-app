using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Services.Repositories.EF
{
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
            return Query().Include(c => c.TaskItems).Include(c => c.User);
        }

        public override Task Update(Task entity)
        {
            //Remuevo previo set de items de la Task. El usuario puede haber creado, eliminado o editado existentes
            var previousItems = _dbContext.TaskItems.Where(t => t.TaskId == entity.Id);
            _dbContext.TaskItems.RemoveRange(previousItems);

            foreach (var item in entity.TaskItems)
            {
                _dbContext.TaskItems.Add(item);
            }

            return base.Update(entity);
        }
    }
}
