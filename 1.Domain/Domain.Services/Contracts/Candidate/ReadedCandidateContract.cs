using Domain.Model.Enum;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.CandidateSkill;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.OpenPositions;
using Domain.Services.Contracts.User;
using System;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Candidate
{
    public class ReadedCandidateContract
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
        public ReadedUserContract User { get; set; }
        public ReadedCommunityContract Community { get; set; }
        public ReadedCandidateProfileContract Profile { get; set; }
        public bool IsReferred { get; set; }
        public int PreferredOfficeId { get; set; }
        public DateTime ContactDay { get; set; }
        public ICollection<ReadedCandidateSkillContract> CandidateSkills { get; set; }
        public string Cv { get; set; }
        public string KnownFrom { get; set; }
        public string ReferredBy { get; set; }
        public ReadedOpenPositionContract OpenPosition { get; set; }
        public string OpenPositionTitle { get; set; }
    }
}
