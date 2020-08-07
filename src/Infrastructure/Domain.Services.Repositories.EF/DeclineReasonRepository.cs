// <copyright file="DeclineReasonRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class DeclineReasonRepository : Repository<DeclineReason, DataBaseContext>
    {
        public DeclineReasonRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<DeclineReason> Query()
        {
            return base.Query();
        }

        public override IQueryable<DeclineReason> QueryEager()
        {
            return this.Query();
        }
    }
}
