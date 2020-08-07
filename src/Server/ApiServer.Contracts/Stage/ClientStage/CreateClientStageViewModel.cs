// <copyright file="CreateClientStageViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Stage
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.Interview;
    using ApiServer.Contracts.ReaddressStatus;
    using Domain.Model.Enum;

    public class CreateClientStageViewModel
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public int? UserOwnerId { get; set; }

        public int? UserDelegateId { get; set; }

        public string RejectionReason { get; set; }

        public string Interviewer { get; set; }

        public string DelegateName { get; set; }

        public IList<CreateInterviewViewModel> Interviews { get; set; }

        public CreateReaddressStatusViewModel ReaddressStatus { get; set; }
    }
}
