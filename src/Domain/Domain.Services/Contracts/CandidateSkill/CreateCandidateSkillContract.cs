// <copyright file="CreateCandidateSkillContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.CandidateSkill
{
    using Domain.Services.Contracts.Candidate;
    using Domain.Services.Contracts.Skill;

    public class CreateCandidateSkillContract
    {
        public int CandidateId { get; set; }

        public CreateCandidateContract Candidate { get; set; }

        public int SkillId { get; set; }

        public CreateSkillContract Skill { get; set; }

        public int Rate { get; set; }

        public string Comment { get; set; }
    }
}
