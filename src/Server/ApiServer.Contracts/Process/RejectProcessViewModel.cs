// <copyright file="RejectProcessViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Process
{
    public class RejectProcessViewModel
    {
        public int Id { get; set; }

        public string RejectionReason { get; set; }
    }
}
