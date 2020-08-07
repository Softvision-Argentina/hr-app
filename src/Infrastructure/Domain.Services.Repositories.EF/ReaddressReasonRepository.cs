// <copyright file="ReaddressReasonRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class ReaddressReasonRepository : Repository<ReaddressReason, DataBaseContext>
    {
        public ReaddressReasonRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<ReaddressReason> QueryEager()
        {
            return this.Query().Include(_ => _.Type);
        }

        public override bool Exist(int id)
        {
            return this.Query().AsNoTracking().FirstOrDefault(_ => _.Id == id) != null;
        }
    }
}
