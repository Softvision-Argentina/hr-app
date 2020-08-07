// <copyright file="ReaddressStatus.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class ReaddressStatusRepository : Repository<ReaddressStatus, DataBaseContext>
    {
        public ReaddressStatusRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override bool Exist(int id)
        {
            return this.Query().AsNoTracking().FirstOrDefault(_ => _.Id == id) != null;
        }
    }
}
