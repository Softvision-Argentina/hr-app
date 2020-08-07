// <copyright file="ReadedProcessViewModel.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Contracts.Process
{
    using System;
    using ApiServer.Contracts.Candidates;
    using ApiServer.Contracts.Postulant;
    using ApiServer.Contracts.Stage;
    using ApiServer.Contracts.User;
    using Domain.Model;
    using Domain.Model.Enum;

    public class ReadedProcessViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ProcessStatus Status { get; set; }

        public ProcessCurrentStage CurrentStage { get; set; }

        public string RejectionReason { get; set; }

        public DeclineReason DeclineReason { get; set; }

        public int? CandidateId { get; set; }

        public ReadedCandidateViewModel Candidate { get; set; }

        public ReadedPostulantViewModel Postulant { get; set; }

        public int? UserOwnerId { get; set; }

        public ReadedUserViewModel UserOwner { get; set; }

        public int? UserDelegateId { get; set; }

        public ReadedUserViewModel UserDelegate { get; set; }

        public float? ActualSalary { get; set; }

        public float? WantedSalary { get; set; }

        public EnglishLevel EnglishLevel { get; set; }

        public Seniority Seniority { get; set; }

        public DateTime HireDate { get; set; }

        public ReadedHrStageViewModel HrStage { get; set; }

        public ReadedTechnicalStageViewModel TechnicalStage { get; set; }

        public ReadedClientStageViewModel ClientStage { get; set; }

        public ReadedPreOfferStageViewModel PreOfferStage { get; set; }

        public ReadedOfferStageViewModel OfferStage { get; set; }
    }
}
