using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Stage.StageItem;
using System;
using System.Collections.Generic;
using Domain.Services.Contracts.Consultant;
using Domain.Model.Enum;

namespace Domain.Services.Contracts.Stage
{
    public class ReadedHrStageContract
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
        public float? ActualSalary { get; set; }
        public float? WantedSalary { get; set; }
        public EnglishLevel? EnglishLevel { get; set; }
        public RejectionReasonsHr RejectionReasonsHr { get; set; }

    }
}