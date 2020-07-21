using ApiServer.Contracts.Interview;
using ApiServer.Contracts.ReaddressStatus;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace ApiServer.Contracts.Stage
{
    public class UpdateClientStageViewModel
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public DateTime? Date { get; set; }
        public StageStatus Status { get; set; }
        public string Feedback { get; set; }
        public int? UserOwnerId { get; set; }
        public int? UserDelegateId { get; set; }
        public string RejectionReason { get; set; }
        public string Interviewer { get; set; }
        public string DelegateName { get; set; }
        public IList<UpdateInterviewViewModel> Interviews { get; set; }
        public UpdateReaddressStatusViewModel ReaddressStatus { get; set; }
    }
}
