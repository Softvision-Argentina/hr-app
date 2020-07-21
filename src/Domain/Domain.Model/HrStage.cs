using Core;
using Domain.Model.Enum;
using System;

namespace Domain.Model
{
    public class HrStage : Entity<int>
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
        public float ActualSalary { get; set; }
        public float WantedSalary { get; set; }
        public string AdditionalInformation { get; set; }
        public EnglishLevel EnglishLevel { get; set; }
        public RejectionReasonsHr RejectionReasonsHr { get; set; }
        public bool SentEmail { get; set; }
        public int? ReaddressStatusId { get; set; }
        public ReaddressStatus ReaddressStatus { get; set; }
    }
}
