// <copyright file="TaskItemRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class TaskItemRepository : Repository<TaskItem, DataBaseContext>
    {
        public TaskItemRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<TaskItem> Query()
        {
            return base.Query();
        }
    }
}
