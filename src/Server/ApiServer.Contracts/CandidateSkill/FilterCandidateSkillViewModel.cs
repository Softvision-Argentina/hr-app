// <copyright file="FilterCandidateSkillViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.CandidateSkill
{
    public class FilterCandidateSkillViewModel
    {
        public int SkillId { get; set; }

        public int MinRate { get; set; }

        public int MaxRate { get; set; }
    }
}
