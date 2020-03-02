using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.Stage;
using Domain.Model;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace ApiServer.Contracts.Process
{
    public class UpdateProcessViewModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        
        public ProcessStatus Status { get; set; }
        public ProcessCurrentStage CurrentStage { get; set; }

        public string RejectionReason { get; set; }
        public DeclineReason DeclineReason { get; set; }

        public int? CandidateId { get; set; }

        public UpdateCandidateViewModel Candidate { get; set; }

        public int? ConsultantOwnerId { get; set; }

        public int? ConsultantDelegateId { get; set; }

        public float? ActualSalary { get; set; }
        public float? WantedSalary { get; set; }
        public float? AgreedSalary { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public Seniority Seniority { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime HireDate { get; set; }

        //public List<UpdateStageViewModel> Stages { get; set; }
        public UpdateHrStageViewModel HrStage { get; set; }
        public UpdateTechnicalStageViewModel TechnicalStage { get; set; }
        public UpdateClientStageViewModel ClientStage { get; set; }
        public UpdateOfferStageViewModel OfferStage { get; set; }
    }
}
