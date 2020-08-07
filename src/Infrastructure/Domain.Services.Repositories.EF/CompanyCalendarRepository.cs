// <copyright file="CompanyCalendarRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class CompanyCalendarRepository : Repository<CompanyCalendar, DataBaseContext>
    {
        public CompanyCalendarRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<CompanyCalendar> Query()
        {
            return base.Query();
        }

        public override IQueryable<CompanyCalendar> QueryEager()
        {
            return this.Query();
        }
    }
}
