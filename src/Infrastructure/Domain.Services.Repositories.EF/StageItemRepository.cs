// <copyright file="StageItemRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Interfaces.Repositories;
    using Persistance.EF;

    public class StageItemRepository : Repository<StageItem, DataBaseContext>, IStageItemRepository
    {
        public StageItemRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<StageItem> Query()
        {
            return this.DbContext.StageItems;
        }

        public void UpdateStageItem(StageItem newStageItem, StageItem existingStageItem)
        {
            this.DbContext.Entry(existingStageItem).CurrentValues.SetValues(newStageItem);
        }
    }
}
