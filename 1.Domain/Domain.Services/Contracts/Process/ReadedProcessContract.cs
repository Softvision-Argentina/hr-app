using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Consultant;
using Domain.Services.Contracts.Stage;
using System;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Process
{
    public class ReadedProcessContract
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ProcessStatus Status { get; set; }
        public ProcessCurrentStage CurrentStage { get; set; }

        public string Profile { get; set; }

        public string RejectionReason { get; set; }
        public DeclineReason DeclineReason { get; set; }
        public int? CandidateId { get; set; }
        public ReadedCandidateContract Candidate { get; set; }

        public int? ConsultantOwnerId { get; set; }
        public ReadedConsultantContract ConsultantOwner { get; set; }

        public int? ConsultantDelegateId { get; set; }
        public ReadedConsultantContract ConsultantDelegate { get; set; }

        public float? ActualSalary { get; set; }
        public float? WantedSalary { get; set; }
        public float? AgreedSalary { get; set; }

        public string EnglishLevel { get; set; }
        public Seniority Seniority { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime HireDate { get; set; }

        //public List<ReadedStageContract> Stages { get; set; }

        public ReadedHrStageContract HrStage { get; set; }
        public ReadedTechnicalStageContract TechnicalStage { get; set; }
        public ReadedClientStageContract ClientStage { get; set; }
        public ReadedOfferStageContract OfferStage { get; set; }
    }
}
