﻿// <copyright file="CreatePreOfferStageViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Stage
{
    using System;
    using ApiServer.Contracts.ReaddressStatus;
    using Domain.Model.Enum;

    public class CreatePreOfferStageViewModel
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public int? UserOwnerId { get; set; }

        public int? UserDelegateId { get; set; }

        public string RejectionReason { get; set; }

        public int DNI { get; set; }

        public bool? BackgroundCheckDone { get; set; }

        public DateTime? BackgroundCheckDoneDate { get; set; }

        public bool? PreocupationalDone { get; set; }

        public DateTime? PreocupationalDoneDate { get; set; }

        public int RemunerationOffer { get; set; }

        public int VacationDays { get; set; }

        public DateTime Firstday { get; set; }

        public string Bonus { get; set; }

        public string Notes { get; set; }

        public HealthInsuranceEnum HealthInsurance { get; set; }

        public CreateReaddressStatusViewModel ReaddressStatus { get; set; }
    }
}
