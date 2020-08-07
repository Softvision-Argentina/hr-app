// <copyright file="ReadedOpenPositionViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.OpenPosition
{
    using ApiServer.Contracts.Community;
    using Domain.Model.Enum;

    public class ReadedOpenPositionViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Seniority Seniority { get; set; }

        public string Studio { get; set; }

        public ReadedCommunityViewModel Community { get; set; }

        public bool Priority { get; set; }

        public string JobDescription { get; set; }
    }
}
