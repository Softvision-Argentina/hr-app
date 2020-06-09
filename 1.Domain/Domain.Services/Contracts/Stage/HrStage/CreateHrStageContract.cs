using Domain.Model.Enum;
using Domain.Services.Contracts.Stage.StageItem;
using System;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Stage
{
    public class CreateHrStageContract
    {
        public int ProcessId { get; set; }
        public DateTime? Date { get; set; }
        public StageStatus Status { get; set; }
        public string Feedback { get; set; }
        public List<CreateStageItemContract> StageItems { get; set; }
        public int? UserOwnerId { get; set; }
        public int? UserDelegateId { get; set; }
        public string RejectionReason { get; set; }
        public float ActualSalary { get; set; }
        public float WantedSalary { get; set; }
        public string AdditionalInformation { get; set; }
        public EnglishLevel EnglishLevel { get; set; }
        public RejectionReasonsHr RejectionReasonsHr { get; set; }
        public bool SentEmail { get; set; }
    }
}
