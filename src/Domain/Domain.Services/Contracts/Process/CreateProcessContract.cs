﻿// <copyright file="CreateProcessContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Process
{
    using System;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Candidate;
    using Domain.Services.Contracts.Stage;

    public class CreateProcessContract
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ProcessStatus Status { get; set; }

        public ProcessCurrentStage CurrentStage { get; set; }

        public string Profile { get; set; }

        public string RejectionReason { get; set; }

        public DeclineReason DeclineReason { get; set; }

        public int? CandidateId { get; set; }

        public int? UserOwnerId { get; set; }

        public int? UserDelegateId { get; set; }

        public float? ActualSalary { get; set; }

        public float? WantedSalary { get; set; }

        public string EnglishLevel { get; set; }

        public Seniority Seniority { get; set; }

        public DateTime HireDate { get; set; }

        public UpdateCandidateContract Candidate { get; set; }

        public CreateHrStageContract HrStage { get; set; }

        public CreateTechnicalStageContract TechnicalStage { get; set; }

        public CreateClientStageContract ClientStage { get; set; }

        public CreatePreOfferStageContract PreOfferStage { get; set; }

        public CreateOfferStageContract OfferStage { get; set; }
    }
}
