// <copyright file="ReadedCompanyCalendarContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.CompanyCalendar
{
    using System;

    public class ReadedCompanyCalendarContract
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public string Comments { get; set; }
    }
}
