// <copyright file="CreateReaddressStatus.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.ReaddressStatus
{
    using Domain.Model.Enum;

    public class CreateReaddressStatus
    {
        public StageStatus FromStatus { get; set; }

        public StageStatus ToStatus { get; set; }

        public int ReaddressReasonId { get; set; }

        public string Feedback { get; set; }
    }
}
