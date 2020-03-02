using Core;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class OfferStage: Entity<int>
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

        public DateTime OfferDate { get; set; }
        public DateTime HireDate { get; set; }
        public Seniority Seniority { get; set; }      
        public bool BackgroundCheckDone { get; set; }
        public DateTime? BackgroundCheckDoneDate { get; set; }
        public bool PreocupationalDone { get; set; }
        public DateTime? PreocupationalDoneDate { get; set; }        
    }
}
