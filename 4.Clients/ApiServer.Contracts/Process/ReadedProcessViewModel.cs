using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.Consultant;
using ApiServer.Contracts.Postulant;
using ApiServer.Contracts.Stage;
using Domain.Model;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace ApiServer.Contracts.Process
{
    public class ReadedProcessViewModel
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ProcessStatus Status { get; set; }
        public ProcessCurrentStage CurrentStage { get; set; }

        public string RejectionReason { get; set; }
        public DeclineReason DeclineReason { get; set; }
        public int? CandidateId { get; set; }
        public ReadedCandidateViewModel Candidate { get; set; }
        public ReadedPostulantViewModel Postulant { get; set; }

        public int? ConsultantOwnerId { get; set; }
        public ReadedConsultantViewModel ConsultantOwner { get; set; }

        public int? ConsultantDelegateId { get; set; }
        public ReadedConsultantViewModel ConsultantDelegate { get; set; }

        public float? ActualSalary { get; set; }
        public float? WantedSalary { get; set; }
        public float? AgreedSalary { get; set; }
        public EnglishLevel EnglishLevel { get; set; }
        public Seniority Seniority { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime HireDate { get; set; }

        //public List<ReadedStageViewModel> Stages { get; set; }
        public ReadedHrStageViewModel HrStage { get; set; }
        public ReadedTechnicalStageViewModel TechnicalStage { get; set; }
        public ReadedClientStageViewModel ClientStage { get; set; }
        public ReadedOfferStageViewModel OfferStage { get; set; }
    }
}