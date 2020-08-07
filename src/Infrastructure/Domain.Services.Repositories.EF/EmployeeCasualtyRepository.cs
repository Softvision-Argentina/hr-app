// <copyright file="EmployeeCasualtyRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class EmployeeCasualtyRepository : Repository<EmployeeCasualty, DataBaseContext>
    {
        public EmployeeCasualtyRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<EmployeeCasualty> Query()
        {
            return base.Query();
        }

        public override IQueryable<EmployeeCasualty> QueryEager()
        {
            return this.Query();
        }
    }
}
