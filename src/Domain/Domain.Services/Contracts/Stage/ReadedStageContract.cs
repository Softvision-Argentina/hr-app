// <copyright file="ReadedStageContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Stage
{
    using System;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.User;

    public class ReadedStageContract
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
    }
}
