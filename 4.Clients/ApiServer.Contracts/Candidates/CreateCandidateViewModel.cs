using ApiServer.Contracts.CandidateProfile;
using ApiServer.Contracts.CandidateSkill;
using ApiServer.Contracts.Community;
using ApiServer.Contracts.User;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace ApiServer.Contracts.Candidates
{
    public class CreateCandidateViewModel
    {
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
        public ICollection<CreateCandidateSkillViewModel> CandidateSkills { get; set; }
        public string Cv { get; set; }
        public string KnownFrom { get; set; }
        public string ReferredBy { get; set; }
    }
}
