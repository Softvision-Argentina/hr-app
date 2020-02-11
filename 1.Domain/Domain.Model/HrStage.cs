using Core;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class HrStage : Entity<int>
    {
        public StageType Type { get; set; }

        public DateTime? Date { get; set; }

        public string Feedback { get; set; }

        public StageStatus Status { get; set; }

        public int ProcessId { get; set; }
        public Process Process { get; set; }
        public int? ConsultantOwnerId { get; set; }
        public Consultant ConsultantOwner { get; set; }
        public int? ConsultantDelegateId { get; set; }
        public Consultant ConsultantDelegate { get; set; }
        public string RejectionReason { get; set; }

        public float ActualSalary { get; set; }
        public float WantedSalary { get; set; }
        public EnglishLevel EnglishLevel { get; set; }
        public RejectionReasonsHr RejectionReasonsHr { get; set; }
    }
}
