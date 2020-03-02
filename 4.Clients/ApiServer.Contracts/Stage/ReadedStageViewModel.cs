using System;
using System.Collections.Generic;
using System.Text;
using ApiServer.Contracts.Consultant;
using Domain.Model.Enum;

namespace ApiServer.Contracts.Stage
{
    public class ReadedStageViewModel
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public int? ConsultantOwnerId { get; set; }
        public ReadedConsultantViewModel ConsultantOwner { get; set; }

        public int? ConsultantDelegateId { get; set; }
        public ReadedConsultantViewModel ConsultantDelegate { get; set; }
        public string RejectionReason { get; set; }
    }
}
