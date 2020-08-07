// <copyright file="UpdateCandidateContractValidatorTester.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Validators.Candidate
{
    using Domain.Services.Impl.Validators.Candidate;
    using FluentValidation.TestHelper;
    using Xunit;

    public class UpdateCandidateContractValidatorTester
    {
        private readonly UpdateCandidateContractValidator validator;

        public UpdateCandidateContractValidatorTester()
        {
            this.validator = new UpdateCandidateContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Name, (string)null);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Name, string.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is null")]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.LastName, (string)null);
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is empty")]
        public void Should_Have_Error_When_LastName_Is_Blank()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.LastName, string.Empty);
        }
    }
}
