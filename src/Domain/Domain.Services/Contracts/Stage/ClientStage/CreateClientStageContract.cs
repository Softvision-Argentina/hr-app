// <copyright file="CreateClientStageContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Stage
{
    using System;
    using System.Collections.Generic;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Interview;
    using Domain.Services.Contracts.ReaddressStatus;
    using Domain.Services.Contracts.Stage.StageItem;

    public class CreateClientStageContract
    {
        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public List<CreateStageItemContract> StageItems { get; set; }

        public int? UserOwnerId { get; set; }

        public int? UserDelegateId { get; set; }

        public string RejectionReason { get; set; }

        public string Interviewer { get; set; }

        public string DelegateName { get; set; }

        public IList<CreateInterviewContract> Interviews { get; set; }

        public CreateReaddressStatus ReaddressStatus { get; set; }
    }
}
