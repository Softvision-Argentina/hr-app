// <copyright file="CreateCandidateProfileViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.CandidateProfile
{
    using System.Collections.Generic;
    using ApiServer.Contracts.Community;

    public class CreateCandidateProfileViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<CreateCommunityViewModel> CommunityItems { get; set; }
    }
}
