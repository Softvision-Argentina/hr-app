// <copyright file="ReaddressStatus.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;
    using Domain.Model.Enum;

    public class ReaddressStatus : Entity<int>
    {
        public StageStatus FromStatus { get; set; }

        public StageStatus ToStatus { get; set; }

        public int? ReaddressReasonId { get; set; }

        public ReaddressReason ReaddressReason { get; set; }

        public string Feedback { get; set; }
    }
}
