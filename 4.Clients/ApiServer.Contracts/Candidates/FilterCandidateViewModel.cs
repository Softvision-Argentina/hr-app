using ApiServer.Contracts.CandidateSkill;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.Candidates
{
    public class FilterCandidateViewModel
    {
        public int? Community { get; set; }
        public int? PreferredOffice { get; set; }

        public List<FilterCandidateSkillViewModel> SelectedSkills { get; set; }
    }
}
