// <copyright file="DaysOff.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System;
    using Core;
    using Domain.Model.Enum;

    public class DaysOff : Entity<int>
    {
        public DaysOffStatus Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime EndDate { get; set; }

        public DaysOffType Type { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public string GoogleCalendarEventId { get; set; }
    }
}
