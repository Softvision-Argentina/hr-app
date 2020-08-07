// <copyright file="CreateDaysOffContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.DaysOff
{
    using System;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Employee;

    public class CreateDaysOffContract
    {
        public DaysOffStatus Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime EndDate { get; set; }

        public DaysOffType Type { get; set; }

        public int EmployeeId { get; set; }

        public CreateEmployeeContract Employee { get; set; }
    }
}
