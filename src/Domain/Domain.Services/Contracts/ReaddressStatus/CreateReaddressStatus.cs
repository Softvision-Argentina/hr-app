using Domain.Model.Enum;
using Domain.Services.Contracts.ReaddressReason;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Contracts.ReaddressStatus
{
    public class CreateReaddressStatus
    {
        public StageStatus FromStatus { get; set; }
        public StageStatus ToStatus { get; set; }
        public int ReaddressReasonId { get; set; }
        public string Feedback { get; set; }
    }
}
