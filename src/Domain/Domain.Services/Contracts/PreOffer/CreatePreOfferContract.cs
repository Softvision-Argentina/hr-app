﻿// <copyright file="CreatePreOfferContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.PreOffer
{
    using System;
    using Domain.Model.Enum;

    public class CreatePreOfferContract
    {
        public DateTime? PreOfferDate { get; set; }

        public float Salary { get; set; }

        public PreOfferStatus Status { get; set; }

        public int VacationDays { get; set; }

        public HealthInsuranceEnum HealthInsurance { get; set; }

        public int ProcessId { get; set; }

        public int Bonus { get; set; }

        public string Notes { get; set; }

        public DateTime TentativeStartDate { get; set; }
    }
}
