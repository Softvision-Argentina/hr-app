using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Impl.Validators.CandidateProfile;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Domain.Services.Tests.ValidatorTester.CandidateProfile
{
    public class CreateCandidateProfileContractValidatorTester
    {
        private CreateCandidateProfileContractValidator _validator;
        public CreateCandidateProfileContractValidatorTester()
        {
            _validator = new CreateCandidateProfileContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(CandidateProfileContract => CandidateProfileContract.Name, (string)null);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(CandidateProfileContract => CandidateProfileContract.Name, String.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is null")]
        public void Should_Have_Error_When_Description_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(CandidateProfileContract => CandidateProfileContract.Description, (string)null);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is empty")]
        public void Should_Have_Error_When_Description_Is_Blank()
        {
            _validator.ShouldHaveValidationErrorFor(CandidateProfileContract => CandidateProfileContract.Description, String.Empty);
        }
    }
}
