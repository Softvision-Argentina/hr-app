using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.User;
using Domain.Services.Contracts.Stage;
using System;

namespace Domain.Services.Contracts.Process
{
    public class ReadedProcessContract
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProcessStatus Status { get; set; }
        public ProcessCurrentStage CurrentStage { get; set; }
        public string Profile { get; set; }
        public string RejectionReason { get; set; }
        public DeclineReason DeclineReason { get; set; }
        public int? CandidateId { get; set; }
        public ReadedCandidateContract Candidate { get; set; }
        public int? UserOwnerId { get; set; }
        public ReadedUserContract UserOwner { get; set; }
        public int? UserDelegateId { get; set; }
        public ReadedUserContract UserDelegate { get; set; }
        public float? ActualSalary { get; set; }
        public float? WantedSalary { get; set; }        
        public string EnglishLevel { get; set; }
        public Seniority Seniority { get; set; }        
        public DateTime HireDate { get; set; }
        public ReadedHrStageContract HrStage { get; set; }
        public ReadedTechnicalStageContract TechnicalStage { get; set; }
        public ReadedClientStageContract ClientStage { get; set; }
        public ReadedOfferStageContract OfferStage { get; set; }
    }
}
