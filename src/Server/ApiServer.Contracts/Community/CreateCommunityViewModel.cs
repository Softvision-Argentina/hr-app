// <copyright file="CreateCommunityViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Community
{
    using ApiServer.Contracts.CandidateProfile;

    public class CreateCommunityViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int ProfileId { get; set; }

        public CreateCandidateProfileViewModel Profile { get; set; }
    }
}
