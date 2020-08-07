// <copyright file="PreOfferStage.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System;
    using Core;
    using Domain.Model.Enum;

    public class PreOfferStage : Entity<int>
    {
        public StageType Type { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public int ProcessId { get; set; }

        public Process Process { get; set; }

        public int? UserOwnerId { get; set; }

        public User UserOwner { get; set; }

        public int? UserDelegateId { get; set; }

        public User UserDelegate { get; set; }

        public string RejectionReason { get; set; }

        public int DNI { get; set; }

        public bool BackgroundCheckDone { get; set; }

        public DateTime? BackgroundCheckDoneDate { get; set; }

        public DateTime? BornDate { get; set; }

        public bool PreocupationalDone { get; set; }

        public DateTime? PreocupationalDoneDate { get; set; }

        public int RemunerationOffer { get; set; }

        public int VacationDays { get; set; }

        public DateTime Firstday { get; set; }

        public string Bonus { get; set; }

        public HealthInsuranceEnum HealthInsurance { get; set; }

        public string Notes { get; set; }

        public int? ReaddressStatusId { get; set; }

        public ReaddressStatus ReaddressStatus { get; set; }
    }
}
