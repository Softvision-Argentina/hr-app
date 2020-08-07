// <copyright file="ReadReaddressStatusViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.ReaddressStatus
{
    using Domain.Model.Enum;

    public class ReadReaddressStatusViewModel
    {
        public int Id { get; set; }

        public StageStatus FromStatus { get; set; }

        public StageStatus ToStatus { get; set; }

        public int ReaddressReasonId { get; set; }

        public string Feedback { get; set; }
    }
}
