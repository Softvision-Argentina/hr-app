// <copyright file="Interview.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System;
    using Core;

    public class Interview : Entity<int>
    {
        public string Client { get; set; }

        public string ClientInterviewer { get; set; }

        public DateTime InterviewDate { get; set; }

        public string Feedback { get; set; }

        public string Project { get; set; }

        public ClientStage ClientStage { get; set; }

        public int ClientStageId { get; set; }
    }
}
