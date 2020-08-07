// <copyright file="Cv.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using Core;

    public class Cv : Entity<int>
    {
        public string UrlId { get; set; }

        public Candidate Candidate { get; set; }

        public int CandidateId { get; set; }

        public string PublicId { get; set; }
    }
}
