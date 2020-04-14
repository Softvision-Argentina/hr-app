﻿using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.Stage.StageItem;
using System;
using System.Collections.Generic;
using Domain.Services.Contracts.User;
using Domain.Model.Enum;

namespace Domain.Services.Contracts.Stage
{
    public class ReadedTechnicalStageContract
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
        public Seniority Seniority { get; set; }
        public Seniority AlternativeSeniority { get; set; }
        public string Client { get; set; }
    }
}