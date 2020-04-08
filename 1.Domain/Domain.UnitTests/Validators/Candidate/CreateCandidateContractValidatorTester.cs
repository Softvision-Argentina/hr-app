using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.Consultant;
using Domain.Services.Impl.Validators.Candidate;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Validators.Candidate
{
    public class CreateCandidateContractValidatorTester
    {
        private readonly CreateCandidateContractValidator validator;

        public CreateCandidateContractValidatorTester()
        {
            validator = new CreateCandidateContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when DNI int is zero")]
        public void Should_Have_Error_When_DNI_Is_Zero()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.DNI, 0, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when DNI int is not greater than zero")]
        public void Should_Have_Error_When_DNI_Isnt_Greater_Than_Zero()
        {
            var CandidateContract = new CreateCandidateContract
            {
                DNI = -1,
                Name = "Test",
                LastName = "Test",
                Recruiter = new ReadedConsultantContract(),
                Community = new ReadedCommunityContract(),
                Profile = new ReadedCandidateProfileContract(),
                LinkedInProfile = "Test"
            };

            var result = validator.TestValidate(CandidateContract, "Create")
                .Which
                .Property(x => x.DNI)
                .ShouldHaveValidationError()
                .WithErrorCode("GreaterThanValidator");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {  
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Name, (string) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Name, String.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is null")]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.LastName, (string) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is Blank")]
        public void Should_Have_Error_When_LastName_Is_Blank()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.LastName, String.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Recruiter object is null")]
        public void Should_Have_Error_When_Recruiter_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Recruiter, (ReadedConsultantContract) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Community object is null")]
        public void Should_Have_Error_When_Community_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Community, (ReadedCommunityContract) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Profile object is null")]
        public void Should_Have_Error_When_Profile_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Profile, (ReadedCandidateProfileContract) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LinkedInProfile is null")]
        public void Should_Have_Error_When_LinkedInProfile_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.LinkedInProfile, (string) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LinkedInProfile string is empty")]
        public void Should_Have_Error_When_LinkedInProfile_Is_Blank()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.LinkedInProfile, String.Empty, "Create");
        }
    }
}
