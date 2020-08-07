// <copyright file="ReadedInterviewContract.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Contracts.Interview
{
    using System;
    using Domain.Services.Contracts.Stage;

    public class ReadedInterviewContract
    {
        public int Id { get; set; }

        public string Client { get; set; }

        public string ClientInterviewer { get; set; }

        public DateTime InterviewDate { get; set; }

        public string Feedback { get; set; }

        public string Project { get; set; }

        public ReadedClientStageContract ClientStage { get; set; }

        public int ClientStageId { get; set; }
    }
}
