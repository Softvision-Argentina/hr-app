// <copyright file="OpenPosition.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;
    using Domain.Model.Enum;

    public class OpenPosition : Entity<int>
    {
        public string Title { get; set; }

        public Seniority Seniority { get; set; }

        public string Studio { get; set; }

        public Community Community { get; set; }

        public bool Priority { get; set; }

        public string JobDescription { get; set; }
    }
}
