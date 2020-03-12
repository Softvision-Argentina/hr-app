using Domain.Services.Impl.Validators.Community;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Domain.Services.Tests.ValidatorTester.Community
{
    public class CreateCommunityContractValidatorTester
    {
        private CreateCommunityContractValidator _validator;
        public CreateCommunityContractValidatorTester()
        {
            _validator = new CreateCommunityContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(CommunityContract => CommunityContract.Name, (string) null);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            _validator.ShouldHaveValidationErrorFor(CommunityContract => CommunityContract.Name, String.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is null")]
        public void Should_Have_Error_When_Description_Is_Null()
        {
            _validator.ShouldHaveValidationErrorFor(CommunityContract => CommunityContract.Description, (string) null);
        }

    }
}
