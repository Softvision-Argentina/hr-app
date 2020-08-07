// <copyright file="UpdateCompanyCalendarViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.CompanyCalendar
{
    using System;

    public class UpdateCompanyCalendarViewModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        public string Comments { get; set; }
    }
}
