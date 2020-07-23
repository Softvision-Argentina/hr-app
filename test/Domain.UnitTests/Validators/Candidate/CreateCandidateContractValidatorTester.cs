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
        private readonly CreateCandidateContractValidator validator;

        public CreateCandidateContractValidatorTester()
        {
            validator = new CreateCandidateContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Name, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Blank()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.Name, String.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is null")]
        public void Should_Have_Error_When_LastName_Is_Null()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.LastName, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when LastName string is Blank")]
        public void Should_Have_Error_When_LastName_Is_Blank()
        {
            validator.ShouldHaveValidationErrorFor(CandidateContract => CandidateContract.LastName, String.Empty, "Create");
        }
    }
}
