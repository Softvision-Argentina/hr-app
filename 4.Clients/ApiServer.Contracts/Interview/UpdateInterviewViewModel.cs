using System;

namespace ApiServer.Contracts.Interview
{
    public class UpdateInterviewViewModel
    {
        public int Id { get; set; }
        public String Client { get; set; }
        public String ClientInterviewer { get; set; }
        public DateTime InterviewDate { get; set; }
        public String Feedback { get; set; }
        public String Project { get; set; }
    }
}
