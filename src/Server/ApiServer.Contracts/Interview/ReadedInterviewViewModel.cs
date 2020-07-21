using System;
using ApiServer.Contracts.Stage;

namespace ApiServer.Contracts.Interview
{
    public class ReadedInterviewViewModel
    {
        public int Id { get; set; }
        public String Client { get; set; }
        public String ClientInterviewer { get; set; }
        public DateTime InterviewDate { get; set; }
        public String Feedback { get; set; }
        public String Project { get; set; }
        public ReadedClientStageViewModel ClientStage { get; set; }
        public int ClientStageId { get; set; }
    }
}
