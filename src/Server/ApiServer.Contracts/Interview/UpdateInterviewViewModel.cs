// <copyright file="UpdateInterviewViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Interview
{
    using System;
    using ApiServer.Contracts.Stage;

    public class UpdateInterviewViewModel
    {
        public int Id { get; set; }

        public string Client { get; set; }

        public string ClientInterviewer { get; set; }

        public DateTime InterviewDate { get; set; }

        public string Feedback { get; set; }

        public string Project { get; set; }

        public UpdateClientStageViewModel ClientStage { get; set; }

        public int ClientStageId { get; set; }
    }
}
