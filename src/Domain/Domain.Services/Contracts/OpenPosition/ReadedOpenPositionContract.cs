// <copyright file="ReadedOpenPositionContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.OpenPositions
{
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Community;

    public class ReadedOpenPositionContract
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Seniority Seniority { get; set; }

        public string Studio { get; set; }

        public ReadedCommunityContract Community { get; set; }

        public bool Priority { get; set; }

        public string JobDescription { get; set; }
    }
}
