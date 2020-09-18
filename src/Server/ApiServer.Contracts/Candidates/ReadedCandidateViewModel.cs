// <copyright file="ReadedCandidateViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Candidates
{
    using System;
    using System.Collections.Generic;
    using ApiServer.Contracts.CandidateProfile;
    using ApiServer.Contracts.CandidateSkill;
    using ApiServer.Contracts.Community;
    using ApiServer.Contracts.OpenPosition;
    using ApiServer.Contracts.User;
    using Domain.Model.Enum;

    public class ReadedCandidateViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public int DNI { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string LinkedInProfile { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public CandidateStatus Status { get; set; }

        public ReadedUserViewModel User { get; set; }

        public ReadedCommunityViewModel Community { get; set; }

        public ReadedCandidateProfileViewModel Profile { get; set; }

        public bool IsReferred { get; set; }

        public DateTime ContactDay { get; set; }

        public int PreferredOfficeId { get; set; }

        public ICollection<ReadedCandidateSkillViewModel> CandidateSkills { get; set; }

        public string Cv { get; set; }

        public string KnownFrom { get; set; }

        public string ReferredBy { get; set; }

        public ReadedOpenPositionViewModel OpenPosition { get; set; }

        public string OpenPositionTitle { get; set; }

        public string Source { get; set; }

        public Seniority? Seniority { get; set; }
}
}
