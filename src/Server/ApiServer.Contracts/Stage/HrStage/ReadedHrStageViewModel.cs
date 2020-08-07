// <copyright file="ReadedHrStageViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Stage
{
    using System;
    using ApiServer.Contracts.ReaddressStatus;
    using ApiServer.Contracts.User;
    using Domain.Model.Enum;

    public class ReadedHrStageViewModel
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

        public float ActualSalary { get; set; }

        public float WantedSalary { get; set; }

        public string AdditionalInformation { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public RejectionReasonsHr RejectionReasonsHr { get; set; }

        public bool SentEmail { get; set; }

        public ReadReaddressStatusViewModel ReaddressStatus { get; set; }
    }
}
