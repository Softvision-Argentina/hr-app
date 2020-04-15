using Domain.Services.Contracts.Candidate;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.User;
using Domain.Services.Impl.Validators.Candidate;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Validators.Candidate
{
    public class CreateCandidateContractValidatorTester
    {
        private readonly CreateCandidateContractValidator _validator;

        public CreateCandidateContractValidatorTester()
        {
            _validator = new CreateCandidateContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when DNI int is zero")]
        public void Should_Have_Error_When_DNI_Is_Zero()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.DNI, 0, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when DNI int is not greater than zero")]
        public void Should_Have_Error_When_DNI_Isnt_Greater_Than_Zero()
        {
            var candidateContract = new CreateCandidateContract
            {
                DNI = -1,
                Name = "Test",
                LastName = "Test",
                User = new ReadedUserContract(),
                Community = new ReadedCommunityContract(),
                Profile = new ReadedCandidateProfileContract(),
                LinkedInProfile = "Test"
            };

            var result = _validator.TestValidate(candidateContract, "Create")
                .Which
                .Property(x => x.DNI)
                .ShouldHaveValidationError()
                .WithErrorCode("GreaterThanValidator");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {  
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Name, (string) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Name, String.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is null")]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.LastName, (string) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is Blank")]
        public void Should_Have_Error_When_LastName_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.LastName, String.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when User object is null")]
        public void Should_Have_Error_When_User_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Recruiter, (ReadedConsultantContract) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Community object is null")]
        public void Should_Have_Error_When_Community_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Community, (ReadedCommunityContract) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Profile object is null")]
        public void Should_Have_Error_When_Profile_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Profile, (ReadedCandidateProfileContract) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LinkedInProfile is null")]
        public void Should_Have_Error_When_LinkedInProfile_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.LinkedInProfile, (string) null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LinkedInProfile string is empty")]
        public void Should_Have_Error_When_LinkedInProfile_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.LinkedInProfile, String.Empty, "Create");
        }
    }
}
