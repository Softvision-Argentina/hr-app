// <copyright file="CreateCandidateProfileContractValidatorTester.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Validators.CandidateProfile
{
    using Domain.Services.Impl.Validators.CandidateProfile;
    using FluentValidation.TestHelper;
    using Xunit;

    public class CreateCandidateProfileContractValidatorTester
    {
        private readonly CreateCandidateProfileContractValidator validator;

        public CreateCandidateProfileContractValidatorTester()
        {
            this.validator = new CreateCandidateProfileContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateProfileContract => candidateProfileContract.Name, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateProfileContract => candidateProfileContract.Name, string.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Description string is null")]
        public void Should_Have_Error_When_Description_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateProfileContract => candidateProfileContract.Description, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Description string is empty")]
        public void Should_Have_Error_When_Description_Is_Blank()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateProfileContract => candidateProfileContract.Description, string.Empty, "Create");
        }
    }
}
