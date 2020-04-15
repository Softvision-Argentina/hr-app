using Domain.Model.Enum;
using Domain.Services.Contracts.CandidateSkill;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.User;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.Office;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts.Candidate
{
    public class CreateCandidateContract
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int DNI { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string LinkedInProfile { get; set; }
        public string AdditionalInformation { get; set; }
        public EnglishLevel EnglishLevel { get; set; }
        public CandidateStatus Status { get; set; }
        public ReadedUserContract User { get; set; }
        public ReadedCommunityContract Community { get; set; }
        public ReadedCandidateProfileContract Profile { get; set; }
        public bool IsReferred { get; set; }
        //public int PreferredOfficeId { get; set; }
        public DateTime ContactDay { get; set; }
        public ICollection<CreateCandidateSkillContract> CandidateSkills { get; set; }
        public string Cv { get; set; }
        public string KnownFrom { get; set; }
        public string ReferredBy { get; set; }
    }
}
