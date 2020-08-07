// <copyright file="Community.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;

    public class Community : DescriptiveEntity<int>
    {
        public int ProfileId { get; set; }

        public CandidateProfile Profile { get; set; }
    }
}
