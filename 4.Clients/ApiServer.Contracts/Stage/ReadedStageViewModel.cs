using System;
using System.Collections.Generic;
using System.Text;
using ApiServer.Contracts.User;
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

        public int? UserOwnerId { get; set; }
        public ReadedUserViewModel UserOwner { get; set; }

        public int? UserDelegateId { get; set; }
        public ReadedUserViewModel UserDelegate { get; set; }
        public string RejectionReason { get; set; }
    }
}
