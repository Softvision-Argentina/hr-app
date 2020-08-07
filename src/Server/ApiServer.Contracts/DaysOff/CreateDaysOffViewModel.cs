// <copyright file="CreateDaysOffViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.DaysOff
{
    using System;
    using ApiServer.Contracts.Employee;
    using Domain.Model.Enum;

    public class CreateDaysOffViewModel
    {
        public DaysOffStatus Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime EndDate { get; set; }

        public DaysOffType Type { get; set; }

        public int EmployeeId { get; set; }

        public CreateEmployeeViewModel Employee { get; set; }
    }
}
