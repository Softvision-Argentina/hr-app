// <copyright file="UpdateTechnicalStageContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Stage
{
    using System;
    using System.Collections.Generic;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.ReaddressStatus;
    using Domain.Services.Contracts.Stage.StageItem;

    public class UpdateTechnicalStageContract
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public List<CreateStageItemContract> StageItems { get; set; }

        public int? UserOwnerId { get; set; }

        public int? UserDelegateId { get; set; }

        public string RejectionReason { get; set; }

        public Seniority Seniority { get; set; }

        public Seniority AlternativeSeniority { get; set; }

        public string Client { get; set; }

        public bool SentEmail { get; set; }

        public UpdateReaddressStatus ReaddressStatus { get; set; }
    }
}
