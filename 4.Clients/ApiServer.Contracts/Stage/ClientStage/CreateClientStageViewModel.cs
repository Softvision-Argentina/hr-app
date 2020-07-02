﻿using ApiServer.Contracts.Interview;
using ApiServer.Contracts.ReaddressStatus;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;

namespace ApiServer.Contracts.Stage
{
    public class CreateClientStageViewModel
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
        public IList<CreateInterviewViewModel> Interviews { get; set; }
        public CreateReaddressStatusViewModel ReaddressStatus { get; set; }
    }
}
