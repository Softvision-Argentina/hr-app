// <copyright file="UpdateCandidateProfileContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.CandidateProfile
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Community;
    using Domain.Services.Contracts.SkillProfile;

    public class UpdateCandidateProfileContract
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<CreateCommunityContract> CommunityItems { get; set; }
    }
}
