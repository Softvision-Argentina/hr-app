// <copyright file="Skill.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System.Collections.Generic;
    using Core;

    public class Skill : DescriptiveEntity<int>
    {
        public SkillType Type { get; set; }

        public IList<CandidateSkill> CandidateSkills { get; set; }
    }
}
