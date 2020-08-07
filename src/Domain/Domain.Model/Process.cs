// <copyright file="Process.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Model
{
    using System;
    using Core;
    using Domain.Model.Enum;

    public class Process : Entity<int>
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ProcessStatus Status { get; set; }

        public ProcessCurrentStage CurrentStage { get; set; }

        public string RejectionReason { get; set; }

        public int? DeclineReasonId { get; set; }

        public DeclineReason DeclineReason { get; set; }

        public int? CandidateId { get; set; }

        public Candidate Candidate { get; set; }

        public int? UserOwnerId { get; set; }

        public User UserOwner { get; set; }

        public int? UserDelegateId { get; set; }

        public User UserDelegate { get; set; }

        public float ActualSalary
        {
            get { return this.HrStage.ActualSalary; }
        }

        public float WantedSalary
        {
            get { return this.HrStage.WantedSalary; }
        }

        public EnglishLevel EnglishLevel
        {
            get { return this.HrStage.EnglishLevel; }
        }

        public Seniority Seniority
        {
            get
            {
                return this.OfferStage.Status != StageStatus.NA ? this.OfferStage.Seniority : this.TechnicalStage.Seniority != 0 ? this.TechnicalStage.Seniority : this.TechnicalStage.AlternativeSeniority;
            }
        }

        public DateTime HireDate
        {
            get { return this.OfferStage.HireDate; }
        }

        public HrStage HrStage { get; set; }

        public TechnicalStage TechnicalStage { get; set; }

        public ClientStage ClientStage { get; set; }

        public PreOfferStage PreOfferStage { get; set; }

        public OfferStage OfferStage { get; set; }
    }
}
