using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.Stage;
using Domain.Model;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace ApiServer.Contracts.Process
{
    public class CreateProcessViewModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
       
        public ProcessStatus Status { get; set; }
        public ProcessCurrentStage CurrentStage { get; set; }

        public string RejectionReason { get; set; }
        public DeclineReason DeclineReason { get; set; }

        public int? CandidateId { get; set; }

        public int? ConsultantOwnerId { get; set; }

        public string Interviewer { get; set; }

        public int? ConsultantDelegateId { get; set; }

        public string DelegateName { get; set; }

        public float? ActualSalary { get; set; }

        public float? WantedSalary { get; set; }        

        public EnglishLevel EnglishLevel { get; set; }

        public Domain.Model.Enum.Seniority Seniority { get; set; }        
        public DateTime HireDate { get; set; }

        //public List<CreateStageViewModel> Stages { get; set; }

        public UpdateCandidateViewModel Candidate { get; set; }

        public CreateHrStageViewModel HrStage { get; set; }
        public CreateTechnicalStageViewModel TechnicalStage { get; set; }
        public CreateClientStageViewModel ClientStage { get; set; }
        public CreateOfferStageViewModel OfferStage { get; set; }
    }
}
