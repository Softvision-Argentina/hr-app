using System;
using System.Collections.Generic;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage.StageItem;

namespace Domain.Services.Contracts.Stage
{
    public class UpdateTechnicalStageContract
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public List<CreateStageItemContract> StageItems { get; set; }

        public int? ConsultantOwnerId { get; set; }

        public int? ConsultantDelegateId { get; set; }
        public string RejectionReason { get; set; }

        public Seniority Seniority { get; set; }
        public Seniority AlternativeSeniority { get; set; }
        public string Client { get; set; }
    }
}
