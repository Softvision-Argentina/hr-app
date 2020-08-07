// <copyright file="UpdateCandidateSkillViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.CandidateSkill
{
    using ApiServer.Contracts.Candidates;
    using ApiServer.Contracts.Skills;

    public class UpdateCandidateSkillViewModel
    {
        public int CandidateId { get; set; }

        public ReadedCandidateViewModel Candidate { get; set; }

        public int SkillId { get; set; }

        public ReadedSkillViewModel Skill { get; set; }

        public int Rate { get; set; }

        public string Comment { get; set; }
    }
}
