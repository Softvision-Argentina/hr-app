// <copyright file="ReadedClientStageViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Stage
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.Interview;
    using ApiServer.Contracts.ReaddressStatus;
    using ApiServer.Contracts.User;
    using Domain.Model.Enum;

    public class ReadedClientStageViewModel
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public int? UserOwnerId { get; set; }

        public ReadedUserViewModel UserOwner { get; set; }

        public int? UserDelegateId { get; set; }

        public ReadedUserViewModel UserDelegate { get; set; }

        public string RejectionReason { get; set; }

        public string Interviewer { get; set; }

        public string DelegateName { get; set; }

        public IList<ReadedInterviewViewModel> Interviews { get; set; }

        public ReadReaddressStatusViewModel ReaddressStatus { get; set; }
    }
}
