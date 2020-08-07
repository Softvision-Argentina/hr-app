// <copyright file="CreateCandidateSkillViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.CandidateSkill
{
    using ApiServer.Contracts.Candidates;
    using ApiServer.Contracts.Skills;

    public class CreateCandidateSkillViewModel
    {
        public int CandidateId { get; set; }

        public CreateCandidateViewModel Candidate { get; set; }

        public int SkillId { get; set; }

        public CreateSkillViewModel Skill { get; set; }

        public int Rate { get; set; }

        public string Comment { get; set; }
    }
}
