using Core;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class ClientStage: Entity<int>
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

        public string Interviewer { get; set; }
        public string DelegateName { get; set; }
    }
}
