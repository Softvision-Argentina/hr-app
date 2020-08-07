// <copyright file="CreateCandidateViewModelBuilder.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.FunctionalTests.Dummy.Candidates.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ApiServer.Contracts.Candidates;
    using ApiServer.Contracts.CandidateSkill;
    using Domain.Model.Enum;

    public class CreateCandidateViewModelBuilder
    {
        private string name;
        private readonly string additionalInformation;

        private string LastName { get; set; }

        private int Dni { get; set; }

        private string EmailAddress { get; set; }

        private string PhoneNumber { get; set; }

        private string LinkedInProfile { get; set; }

        private EnglishLevel EnglishLevel { get; set; }

        private CandidateStatus Status { get; set; }

        public DateTime ContactDay { get; set; }

        private ICollection<CreateCandidateSkillViewModel> CandidateSkills { get; set; }

        public CreateCandidateViewModelBuilder()
        {
            this.name = $"test {Guid.NewGuid()}";
            this.additionalInformation = $"AdditionalInformation for {this.name}";
            this.LastName = $"AdditionalInformation for {this.name}";
            this.Dni = 34578645;
            this.EmailAddress = $"Email for {this.name}";
            this.PhoneNumber = $"Phone number for {this.name}";
            this.LinkedInProfile = $"Phone number for {this.name}";
            this.EnglishLevel = EnglishLevel.Advanced;
            this.Status = CandidateStatus.InProgress;
            this.CandidateSkills = null;
            this.ContactDay = new DateTime(2019, 6, 1, 7, 47, 0);
        }

        public CreateCandidateViewModel Build()
        {
            return new CreateCandidateViewModel
            {
                Name = this.name,
                LastName = this.LastName,
                DNI = this.Dni,
                EmailAddress = this.EmailAddress,
                PhoneNumber = this.PhoneNumber,
                LinkedInProfile = this.LinkedInProfile,
                EnglishLevel = this.EnglishLevel,
                Status = this.Status,
                CandidateSkills = this.CandidateSkills,
                ContactDay = this.ContactDay,
            };
        }

        internal CreateCandidateViewModelBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        internal CreateCandidateViewModelBuilder SetLastName(string lastName)
        {
            this.LastName = lastName;
            return this;
        }

        internal CreateCandidateViewModelBuilder WithDNI(int dni)
        {
            this.Dni = dni;
            return this;
        }
    }
}
