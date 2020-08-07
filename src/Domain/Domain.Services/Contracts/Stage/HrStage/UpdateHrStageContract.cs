// <copyright file="UpdateHrStageContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Stage
{
    using System;
    using System.Collections.Generic;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.ReaddressStatus;
    using Domain.Services.Contracts.Stage.StageItem;

    public class UpdateHrStageContract
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public List<CreateStageItemContract> StageItems { get; set; }

        public int? UserOwnerId { get; set; }

        public int? UserDelegateId { get; set; }

        public string RejectionReason { get; set; }

        public float ActualSalary { get; set; }

        public float WantedSalary { get; set; }

        public string AdditionalInformation { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public RejectionReasonsHr RejectionReasonsHr { get; set; }

        public bool SentEmail { get; set; }

        public UpdateReaddressStatus ReaddressStatus { get; set; }
    }
}
