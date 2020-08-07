// <copyright file="ICompanyCalendarService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.CompanyCalendar;

    public interface ICompanyCalendarService
    {
        CreatedCompanyCalendarContract Create(CreateCompanyCalendarContract contract);

        ReadedCompanyCalendarContract Read(int id);

        void Update(UpdateCompanyCalendarContract contract);

        void Delete(int id);

        IEnumerable<ReadedCompanyCalendarContract> List();
    }
}
