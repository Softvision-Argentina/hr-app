using Domain.Model.Enum;
using Domain.Services.Contracts.ReaddressStatus;
using Domain.Services.Contracts.User;
using System;

namespace Domain.Services.Contracts.Stage
{
    public class ReadedHrStageContract
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public DateTime? Date { get; set; }
        public StageStatus Status { get; set; }
        public string Feedback { get; set; }
        public int? UserOwnerId { get; set; }
        public ReadedUserContract UserOwner { get; set; }
        public int? UserDelegateId { get; set; }
        public ReadedUserContract UserDelegate { get; set; }
        public string RejectionReason { get; set; }
        public float? ActualSalary { get; set; }
        public float? WantedSalary { get; set; }
        public string AdditionalInformation { get; set; }
        public EnglishLevel? EnglishLevel { get; set; }
        public RejectionReasonsHr RejectionReasonsHr { get; set; }
        public bool SentEmail { get; set; }
        public ReadReaddressStatus ReaddressStatus { get; set; }
    }
}
