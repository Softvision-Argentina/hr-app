// <copyright file="ReadedClientStageContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Stage
{
    using System;
    using System.Collections.Generic;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Interview;
    using Domain.Services.Contracts.ReaddressStatus;
    using Domain.Services.Contracts.User;

    public class ReadedClientStageContract
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public int? UserOwnerId { get; set; }

        public ReadedUserContract UserOwner { get; set; }

        public int? UserDelegateId { get; set; }

        public ReadedUserContract UserDelegate { get; set; }

        public string RejectionReason { get; set; }

        public string Interviewer { get; set; }

        public string DelegateName { get; set; }

        public IList<ReadedInterviewContract> Interviews { get; set; }

        public ReadReaddressStatus ReaddressStatus { get; set; }
    }
}
