// <copyright file="CandidateSkill.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    public class CandidateSkill
    {
        public int CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        public int Rate { get; set; }

        public string Comment { get; set; }
    }
}
