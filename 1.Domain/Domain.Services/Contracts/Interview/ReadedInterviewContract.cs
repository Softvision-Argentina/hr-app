using Domain.Services.Contracts.Stage;
using System;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Interview
{
    public class ReadedInterviewContract
    {
        public int Id { get; set; }
        public String Client { get; set; }
        public String ClientInterviewer { get; set; }
        public DateTime InterviewDate { get; set; }
        public String Feedback { get; set; }
        public String Project { get; set; }
        public ReadedClientStageContract ClientStage  { get; set; }
        public int ClientStageId { get; set; }
 
    }
}
