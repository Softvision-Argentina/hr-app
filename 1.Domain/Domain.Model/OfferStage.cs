using Core;
using Domain.Model.Enum;
using System;

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
        public int? UserOwnerId { get; set; }
        public User UserOwner { get; set; }
        public int? UserDelegateId { get; set; }
        public User UserDelegate { get; set; }
        public string RejectionReason { get; set; }
        public DateTime HireDate { get; set; }
        public Seniority Seniority { get; set; }      
        public bool BackgroundCheckDone { get; set; }
        public DateTime? BackgroundCheckDoneDate { get; set; }
        public bool PreocupationalDone { get; set; }
        public DateTime? PreocupationalDoneDate { get; set; }
        public int RemunerationOffer { get; set; }
        public int VacationDays { get; set; }
        public DateTime Firstday { get; set; }
        public string Bonus { get; set; }
        public HealthInsuranceEnum HealthInsurance { get; set; }
        public string Notes { get; set; }
    }
}
