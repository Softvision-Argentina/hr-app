using Domain.Services.Impl.Validators.CandidateProfile;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Domain.Services.Impl.UnitTests.ValidatorTester.CandidateProfile
{
    public class CreateCandidateProfileContractValidatorTester
    {
        private readonly CreateCandidateProfileContractValidator _validator;

        public CreateCandidateProfileContractValidatorTester()
        {
            _validator = new CreateCandidateProfileContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(candidateProfileContract => candidateProfileContract.Name, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(candidateProfileContract => candidateProfileContract.Name, String.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Description string is null")]
        public void Should_Have_Error_When_Description_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(candidateProfileContract => candidateProfileContract.Description, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Description string is empty")]
        public void Should_Have_Error_When_Description_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(candidateProfileContract => candidateProfileContract.Description, String.Empty, "Create");
        }
    }
}
