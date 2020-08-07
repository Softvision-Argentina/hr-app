// <copyright file="Candidate.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Domain.Model.Enum;

    public class Candidate : Entity<int>
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public int DNI { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string LinkedInProfile { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public CandidateStatus Status { get; set; }

        public Community Community { get; set; }

        public CandidateProfile Profile { get; set; }

        public bool IsReferred { get; set; }

        public Office PreferredOffice { get; set; }

        public DateTime ContactDay { get; set; }

        public IList<CandidateSkill> CandidateSkills { get; set; }

        public string Cv { get; set; }

        public string KnownFrom { get; set; }

        public User User { get; set; }

        public string ReferredBy { get; set; }

        public OpenPosition OpenPosition { get; set; }

        public string PositionTitle { get; set; }

        public string Source { get; set; }
    }
}
