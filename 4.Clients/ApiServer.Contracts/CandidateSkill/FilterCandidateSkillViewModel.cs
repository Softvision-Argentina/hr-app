﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.CandidateSkill
{
    public class FilterCandidateSkillViewModel
    {
        public int SkillId { get; set; }
        public int MinRate{ get; set; }
        public int MaxRate { get; set; }
    }
}
