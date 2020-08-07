// <copyright file="CreateCompanyCalendarContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.CompanyCalendar
{
    using System;

    public class CreateCompanyCalendarContract
    {
        public string Type { get; set; }

        public DateTime Date { get; set; }

        public string Comments { get; set; }
    }
}
