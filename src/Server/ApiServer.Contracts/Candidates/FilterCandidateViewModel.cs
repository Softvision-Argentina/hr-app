// <copyright file="FilterCandidateViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Candidates
{
    using System.Collections.Generic;
    using ApiServer.Contracts.CandidateSkill;

    public class FilterCandidateViewModel
    {
        public int? Community { get; set; }

        public int? PreferredOffice { get; set; }

        public List<FilterCandidateSkillViewModel> SelectedSkills { get; set; }
    }
}
