using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Contracts.Community;
using Domain.Services.Contracts.User;
using Domain.Services.Impl.Validators.Candidate;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Validators.Candidate
{
    public class UpdateCandidateContractValidatorTester
    {
        private readonly UpdateCandidateContractValidator validator;

        public UpdateCandidateContractValidatorTester()
        {
            validator = new UpdateCandidateContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Name, (string) null);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Name, String.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is null")]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.LastName, (string) null);
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is empty")]
        public void Should_Have_Error_When_LastName_Is_Blank()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.LastName, String.Empty);
        }
    }
}
