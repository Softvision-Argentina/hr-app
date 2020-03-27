using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Contracts.Stage
{
    public class CreateTechnicalStageViewModel
    {
        public int Id { get; set; }
        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public int? UserOwnerId { get; set; }

        public int? UserDelegateId { get; set; }
        public string RejectionReason { get; set; }

        public Seniority Seniority { get; set; }
        public Seniority AlternativeSeniority { get; set; }
        public string Client { get; set; }
    }
}
