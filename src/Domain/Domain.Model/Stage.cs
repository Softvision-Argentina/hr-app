﻿// <copyright file="Stage.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System;
    using Core;
    using Domain.Model.Enum;

    public class Stage : Entity<int>
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
    }
}
