using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Stage.StageItem;
using System;
using System.Collections.Generic;
using Domain.Services.Contracts.Consultant;
using Domain.Model.Enum;

namespace Domain.Services.Contracts.Stage
{
    public class ReadedOfferStageContract
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public int? ConsultantOwnerId { get; set; }
        public ReadedConsultantContract ConsultantOwner { get; set; }

        public int? ConsultantDelegateId { get; set; }
        public ReadedConsultantContract ConsultantDelegate { get; set; }
        public string RejectionReason { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime HireDate { get; set; }
        public Seniority Seniority { get; set; }
        public float AgreedSalary { get; set; }
        public bool BackgroundCheckDone { get; set; }
        public DateTime? BackgroundCheckDoneDate { get; set; }
        public bool PreocupationalDone { get; set; }
        public DateTime? PreocupationalDoneDate { get; set; }
    }
}