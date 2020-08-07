// <copyright file="CreateCommunityContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Community
{
    using Domain.Services.Contracts.CandidateProfile;

    public class CreateCommunityContract
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int ProfileId { get; set; }

        public CreateCandidateProfileContract Profile { get; set; }
    }
}
