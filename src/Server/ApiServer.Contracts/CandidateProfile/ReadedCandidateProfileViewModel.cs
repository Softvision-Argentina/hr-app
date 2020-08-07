// <copyright file="ReadedCandidateProfileViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.CandidateProfile
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Community;

    public class ReadedCandidateProfileViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ReadedCommunityViewModel> CommunityItems { get; set; }
    }
}
