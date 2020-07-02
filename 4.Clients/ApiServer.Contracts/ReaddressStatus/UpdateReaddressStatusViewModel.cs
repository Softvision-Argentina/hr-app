using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.ReaddressStatus
{
    public class UpdateReaddressStatusViewModel
    {
        public int Id { get; set; }
        public StageStatus FromStatus { get; set; }
        public StageStatus ToStatus { get; set; }
        public int ReaddressReasonId { get; set; }
        public string Feedback { get; set; }
    }
}
