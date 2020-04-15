using Domain.Model;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace HrApp.API.Beans
{
    public class CandidatesResponse
    {
        public ResponseStatus status;
        public string Name { get; set; }
        public string LastName { get; set; }
        public int ? DNI { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string LinkedInProfile { get; set; }
        public string AdditionalInformation { get; set; }
        public EnglishLevel EnglishLevel { get; set; }
        public CandidateStatus Status { get; set; }
        public User User { get; set; }
        public Office PreferredOffice { get; set; }
        public DateTime ContactDay { get; set; }
        public IList<CandidateSkill> CandidateSkills { get; set; }
    }
}
