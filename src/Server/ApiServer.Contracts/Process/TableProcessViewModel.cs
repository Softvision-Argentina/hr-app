// <copyright file="ReadedProcessViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Process
{
    using ApiServer.Contracts.Candidates;
    using ApiServer.Contracts.User;
    using Domain.Model.Enum;

    public class TableProcessViewModel
    {
        public int Id { get; set; }

        public ProcessStatus Status { get; set; }

        public ProcessCurrentStage CurrentStage { get; set; }

        public ReadedCandidateViewModel Candidate { get; set; }

        public ReadedUserViewModel UserOwner { get; set; }

        public Seniority Seniority { get; set; }
    }
}
