using Domain.Model.Enum;
using Domain.Services.Contracts.ReaddressStatus;
using Domain.Services.Contracts.User;
using System;

namespace Domain.Services.Contracts.Stage
{
    public class ReadedPreOfferStageContract
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public DateTime? Date { get; set; }
        public StageStatus Status { get; set; }
        public int? UserOwnerId { get; set; }
        public ReadedUserContract UserOwner { get; set; }
        public int? UserDelegateId { get; set; }
        public ReadedUserContract UserDelegate { get; set; }
        public string RejectionReason { get; set; }        
        public int DNI { get; set; }
        public bool BackgroundCheckDone { get; set; }
        public DateTime? BackgroundCheckDoneDate { get; set; }
        public bool PreocupationalDone { get; set; }
        public DateTime? PreocupationalDoneDate { get; set; }
        public int RemunerationOffer { get; set; }
        public int VacationDays { get; set; }
        public DateTime Firstday { get; set; }
        public string Bonus { get; set; }
        public string Notes { get; set; }
        public HealthInsuranceEnum HealthInsurance { get; set; }
        public ReadReaddressStatus ReaddressStatus { get; set; }
    }
}
