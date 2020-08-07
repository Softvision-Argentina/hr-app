// <copyright file="CreateOfferStageContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Stage
{
    using System;
    using System.Collections.Generic;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Stage.StageItem;

    public class CreateOfferStageContract
    {
        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public List<CreateStageItemContract> StageItems { get; set; }

        public int? UserOwnerId { get; set; }

        public int? UserDelegateId { get; set; }

        public string RejectionReason { get; set; }

        public DateTime HireDate { get; set; }

        public Seniority Seniority { get; set; }

        public bool BackgroundCheckDone { get; set; }

        public DateTime? BackgroundCheckDoneDate { get; set; }

        public bool PreocupationalDone { get; set; }

        public DateTime? PreocupationalDoneDate { get; set; }

        public int RemunerationOffer { get; set; }

        public int VacationDays { get; set; }

        public DateTime Firstday { get; set; }

        public string Bonus { get; set; }

        public string Notes { get; set; }

        public HealthInsuranceEnum HealthInsurance { get; set; }
    }
}
