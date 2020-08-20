// <copyright file="CandidateForHrContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>
namespace Domain.Services.Contracts.Candidate
{
    using Domain.Model.Enum;

    public class CandidateForReferralsContract
    {
        public ReadedCandidateContract Candidate { get; set; }

        public int ProcessId { get; set; }

        public ProcessCurrentStage ProcessCurrentStage { get; set; }

        public ProcessStatus ProcessStatus { get; set; }
    }
}
