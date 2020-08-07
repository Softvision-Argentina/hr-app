// <copyright file="UpdateCommunityViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Community
{
    using ApiServer.Contracts.CandidateProfile;

    public class UpdateCommunityViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int ProfileId { get; set; }

        public UpdateCandidateProfileViewModel Profile { get; set; }
    }
}
