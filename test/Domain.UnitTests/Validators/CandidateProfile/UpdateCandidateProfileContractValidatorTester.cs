// <copyright file="UpdateCandidateProfileContractValidatorTester.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Validators.CandidateProfile
{
    using Domain.Services.Impl.Validators.CandidateProfile;
    using FluentValidation.TestHelper;
    using Xunit;

    public class UpdateCandidateProfileContractValidatorTester
    {
        private readonly UpdateCandidateProfileContractValidator validator;

        public UpdateCandidateProfileContractValidatorTester()
        {
            this.validator = new UpdateCandidateProfileContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when ID int is zero")]
        public void Should_Have_Error_When_ID_Is_Zero()
        {
            this.validator.ShouldHaveValidationErrorFor(updateCandidateProfileContract => updateCandidateProfileContract.Id, 0);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(updateCandidateProfileContract => updateCandidateProfileContract.Name, (string)null);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            this.validator.ShouldHaveValidationErrorFor(updateCandidateProfileContract => updateCandidateProfileContract.Name, string.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is null")]
        public void Should_Have_Error_When_Description_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(updateCandidateProfileContract => updateCandidateProfileContract.Description, (string)null);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is empty")]
        public void Should_Have_Error_When_Description_Is_Blank()
        {
            this.validator.ShouldHaveValidationErrorFor(updateCandidateProfileContract => updateCandidateProfileContract.Description, string.Empty);
        }
    }
}
