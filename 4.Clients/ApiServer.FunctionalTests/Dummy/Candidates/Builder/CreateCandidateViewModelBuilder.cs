using ApiServer.Contracts.Candidates;
using ApiServer.Contracts.CandidateSkill;
using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.FunctionalTests.Dummy.Candidates.Builder
{
    public class CreateCandidateViewModelBuilder
    {
        private string name;
        private string additionalInformation;
        private string lastName { get; set; }
        private int dni { get; set; }
        private string emailAddress { get; set; }
        private string phoneNumber { get; set; }
        private string linkedInProfile { get; set; }
        private EnglishLevel englishLevel { get; set; }
        private CandidateStatus status { get; set; }
        public DateTime contactDay { get; set; }
        private ICollection<CreateCandidateSkillViewModel> candidateSkills { get; set; }

        private readonly string _createdBy = "SomeTestUser";

        public CreateCandidateViewModelBuilder()
        {
            name = $"test {Guid.NewGuid()}";
            additionalInformation = $"AdditionalInformation for {name}";
            lastName = $"AdditionalInformation for {name}";
            dni = 34578645;
            emailAddress = $"Email for {name}";
            phoneNumber = $"Phone number for {name}";
            linkedInProfile = $"Phone number for {name}";
            englishLevel = EnglishLevel.Advanced;
            status = CandidateStatus.InProgress;
            candidateSkills = null;
            contactDay = new DateTime(2019, 6, 1, 7, 47, 0);
        }

        public CreateCandidateViewModel Build()
        {
            return new CreateCandidateViewModel
            {
                Name = name,
                AdditionalInformation = additionalInformation,
                LastName = lastName,
                DNI = dni,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber,
                LinkedInProfile = linkedInProfile,
                EnglishLevel = englishLevel,
                Status = status,
                CandidateSkills = candidateSkills,
                ContactDay = contactDay

            };
        }

        internal CreateCandidateViewModelBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        internal CreateCandidateViewModelBuilder LastName(string lastName)
        {
            this.lastName = lastName;
            return this;
        }

        internal CreateCandidateViewModelBuilder WithDNI(int dni)
        {
            this.dni = dni;
            return this;
        }
    }
}
