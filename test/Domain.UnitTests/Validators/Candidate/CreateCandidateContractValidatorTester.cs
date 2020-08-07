namespace Domain.Services.Impl.UnitTests.Validators.Candidate
{
    using System;
    using Domain.Services.Contracts.Candidate;
    using Domain.Services.Contracts.CandidateProfile;
    using Domain.Services.Contracts.Community;
    using Domain.Services.Contracts.User;
    using Domain.Services.Impl.Validators.Candidate;
    using FluentValidation.TestHelper;
    using Xunit;

    public class CreateCandidateContractValidatorTester
    {
        private readonly CreateCandidateContractValidator validator;

        public CreateCandidateContractValidatorTester()
        {
            this.validator = new CreateCandidateContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Name, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.Name, string.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is null")]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.LastName, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is Blank")]
        public void Should_Have_Error_When_LastName_Is_Blank()
        {
            this.validator.ShouldHaveValidationErrorFor(candidateContract => candidateContract.LastName, string.Empty, "Create");
        }
    }
}
