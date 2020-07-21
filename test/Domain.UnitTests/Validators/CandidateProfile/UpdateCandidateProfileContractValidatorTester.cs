using Domain.Services.Impl.Validators.CandidateProfile;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Validators.CandidateProfile
{
    public class UpdateCandidateProfileContractValidatorTester
    {
        private readonly UpdateCandidateProfileContractValidator _validator;

        public UpdateCandidateProfileContractValidatorTester()
        {
            _validator = new UpdateCandidateProfileContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when ID int is zero")]
        public void Should_Have_Error_When_ID_Is_Zero()
        {
            _validator.ShouldHaveValidationErrorFor(UpdateCandidateProfileContract => UpdateCandidateProfileContract.Id, 0);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(UpdateCandidateProfileContract => UpdateCandidateProfileContract.Name, (string) null);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(UpdateCandidateProfileContract => UpdateCandidateProfileContract.Name, String.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is null")]
        public void Should_Have_Error_When_Description_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(UpdateCandidateProfileContract => UpdateCandidateProfileContract.Description, (string) null);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is empty")]
        public void Should_Have_Error_When_Description_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(UpdateCandidateProfileContract => UpdateCandidateProfileContract.Description, String.Empty);
        }
    }
}
