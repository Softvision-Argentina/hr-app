// <copyright file="CreateInterviewViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Interview
{
    using System;
    using ApiServer.Contracts.Stage;

    public class CreateInterviewViewModel
    {
        public string Client { get; set; }

        public string ClientInterviewer { get; set; }

        public DateTime InterviewDate { get; set; }

        public string Feedback { get; set; }

        public string Project { get; set; }

        public CreateClientStageViewModel ClientStage { get; set; }

        public int ClientStageId { get; set; }
    }
}
