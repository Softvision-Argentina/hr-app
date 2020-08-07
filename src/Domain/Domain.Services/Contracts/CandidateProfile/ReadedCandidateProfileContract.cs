// <copyright file="ReadedCandidateProfileContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.CandidateProfile
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Community;

    public class ReadedCandidateProfileContract
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ReadedCommunityContract> CommunityItems { get; set; }
    }
}
