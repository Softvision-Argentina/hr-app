using Domain.Model.Enum;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.CandidateSkill;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.Office;
using Domain.Services.Contracts.User;
using System;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Interview
{
    public class ReadedInterviewAppContract
    {
        public int Id { get; set; }
        public String Client { get; set; }
        public String ClientInterviewer { get; set; }
        public DateTime InterviewDate { get; set; }
        public String Feedback { get; set; }
        public String Project { get; set; }
 
    }
}
