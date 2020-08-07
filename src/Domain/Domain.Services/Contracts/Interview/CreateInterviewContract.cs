// <copyright file="CreateInterviewContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Interview
{
    using System;
    using Domain.Services.Contracts.Stage.ClientStage;

    public class CreateInterviewContract
    {
        public string Client { get; set; }

        public string ClientInterviewer { get; set; }

        public DateTime InterviewDate { get; set; }

        public string Feedback { get; set; }

        public string Project { get; set; }

        public CreatedClientStageContract ClientStage { get; set; }

        public int ClientStageId { get; set; }
    }
}
