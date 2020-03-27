using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Stage.StageItem;
using System;
using System.Collections.Generic;
using Domain.Services.Contracts.User;
using Domain.Model.Enum;
using Domain.Model;

namespace Domain.Services.Contracts.Stage
{
    public class ReadedOfferStageContract
    {
        public int Id { get; set; }

        public int ProcessId { get; set; }

        public DateTime? Date { get; set; }

        public StageStatus Status { get; set; }

        public string Feedback { get; set; }

        public int? UserOwnerId { get; set; }
        public ReadedUserContract UserOwner { get; set; }

        public int? UserDelegateId { get; set; }
        public ReadedUserContract UserDelegate { get; set; }
        public string RejectionReason { get; set; }        
        public DateTime HireDate { get; set; }
        public Seniority Seniority { get; set; }        
        public bool BackgroundCheckDone { get; set; }
        public DateTime? BackgroundCheckDoneDate { get; set; }
        public bool PreocupationalDone { get; set; }
        public DateTime? PreocupationalDoneDate { get; set; }
    }
}