// <copyright file="UpdateDaysOffContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.DaysOff
{
    using System;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Employee;

    public class UpdateDaysOffContract
    {
        public int Id { get; set; }

        public DaysOffStatus Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime EndDate { get; set; }

        public DaysOffType Type { get; set; }

        public int EmployeeId { get; set; }

        public UpdateEmployeeContract Employee { get; set; }
    }
}
