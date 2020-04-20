using Core;
using Domain.Model.Enum;
using System;

namespace Domain.Model
{
    public class Process : Entity<int>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ProcessStatus Status { get; set; }
        public ProcessCurrentStage CurrentStage { get; set; }
        public string RejectionReason { get; set; }
        public int? DeclineReasonId { get; set; }
        public DeclineReason DeclineReason { get; set; }
        public int? CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public int? UserOwnerId { get; set; }
        public User UserOwner { get; set; }
        public int? UserDelegateId { get; set; }
        public User UserDelegate { get; set; }
        public float ActualSalary { get { return HrStage.ActualSalary; } }
        public float WantedSalary { get { return HrStage.WantedSalary; } }        
        public EnglishLevel EnglishLevel { get { return HrStage.EnglishLevel; } }
        public Seniority Seniority { get {
                return (OfferStage.Status != StageStatus.NA ? OfferStage.Seniority : TechnicalStage.Seniority != 0 ? TechnicalStage.Seniority : TechnicalStage.AlternativeSeniority);
            }
        }
        public DateTime HireDate { get { return OfferStage.HireDate; } }
        public HrStage HrStage { get; set; }
        public TechnicalStage TechnicalStage { get; set; }
        public ClientStage ClientStage { get; set; }
        public OfferStage OfferStage { get; set; }
    }
}
