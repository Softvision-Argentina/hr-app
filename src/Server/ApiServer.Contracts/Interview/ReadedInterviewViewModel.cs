// <copyright file="ReadedInterviewViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Interview
{
    using System;
    using ApiServer.Contracts.Stage;

    public class ReadedInterviewViewModel
    {
        public int Id { get; set; }

        public string Client { get; set; }

        public string ClientInterviewer { get; set; }

        public DateTime InterviewDate { get; set; }

        public string Feedback { get; set; }

        public string Project { get; set; }

        public ReadedClientStageViewModel ClientStage { get; set; }

        public int ClientStageId { get; set; }
    }
}
