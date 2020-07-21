using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Stage;
using Domain.Services.Contracts.User;
using System;

namespace Domain.Services.Contracts.Process
{
    public class UpdateProcessContract
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
        public UpdateCandidateContract Candidate { get; set; }
        public int? UserOwnerId { get; set; }
        public UpdateUserContract UserOwner { get; set; }
        public int? UserDelegateId { get; set; }
        public UpdateUserContract UserDelegate { get; set; }
        public float? ActualSalary { get; set; }
        public float? WantedSalary { get; set; }        
        public string EnglishLevel { get; set; }
        public Seniority Seniority { get; set; }        
        public DateTime HireDate { get; set; }
        public UpdateHrStageContract HrStage { get; set; }
        public UpdateTechnicalStageContract TechnicalStage { get; set; }
        public UpdateClientStageContract ClientStage { get; set; }
        public UpdatePreOfferStageContract PreOfferStage { get; set; }
        public UpdateOfferStageContract OfferStage { get; set; }
    }
}
