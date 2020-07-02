using Domain.Model.Enum;
using Domain.Services.Contracts.Interview;
using Domain.Services.Contracts.ReaddressStatus;
using Domain.Services.Contracts.Stage.StageItem;
using System;
using System.Collections.Generic;

namespace Domain.Services.Contracts.Stage
{
    public class UpdateClientStageContract
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public DateTime? Date { get; set; }
        public StageStatus Status { get; set; }
        public string Feedback { get; set; }
        public List<CreateStageItemContract> StageItems { get; set; }
        public int? UserOwnerId { get; set; }
        public int? UserDelegateId { get; set; }
        public string RejectionReason { get; set; }
        public string Interviewer { get; set; }
        public string DelegateName { get; set; }
        public IList<UpdateInterviewContract> Interviews { get; set; }
        public UpdateReaddressStatus ReaddressStatus { get; set; }

    }
}
