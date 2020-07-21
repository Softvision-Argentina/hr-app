using Core;
using System;

namespace Domain.Model
{
    public class Interview: Entity<int>
    {
        public String Client { get; set; }
        public String ClientInterviewer { get; set; }
        public DateTime InterviewDate { get; set; }
        public String Feedback { get; set; }
        public String Project { get; set; }
        public ClientStage ClientStage  { get; set; }
        public int ClientStageId { get; set; }
    }
}
