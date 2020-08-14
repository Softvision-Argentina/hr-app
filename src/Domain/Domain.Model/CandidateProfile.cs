// <copyright file="CandidateProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System.Collections.Generic;
    using Core;

    public class CandidateProfile : DescriptiveEntity<int>
    {
        public IList<Community> CommunityItems { get; set; }

        public IList<SkillProfile> SkillProfiles { get; set; }
    }
}
