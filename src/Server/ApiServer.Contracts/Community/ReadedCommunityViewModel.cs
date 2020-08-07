// <copyright file="ReadedCommunityViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Community
{
    using ApiServer.Contracts.CandidateProfile;

    public class ReadedCommunityViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ProfileId { get; set; }

        public ReadedCandidateProfileViewModel Profile { get; set; }
    }
}
