﻿// <copyright file="ReadedEmployeeContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Employee
{
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Role;

    public class ReadedEmployeeContract
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public int DNI { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string LinkedInProfile { get; set; }

        public string AdditionalInformation { get; set; }

        public EmployeeStatus Status { get; set; }

        public int UserId { get; set; }

        public ReadedRoleContract Role { get; set; }

        public bool IsReviewer { get; set; }

        public ReadedEmployeeContract Reviewer { get; set; }
    }
}
