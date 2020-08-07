// <copyright file="CreateCommunityContractValidatorTester.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Validators.Community
{
    using Domain.Services.Impl.Validators.Community;
    using FluentValidation.TestHelper;
    using Xunit;

    public class CreateCommunityContractValidatorTester
    {
        private readonly CreateCommunityContractValidator validator;

        public CreateCommunityContractValidatorTester()
        {
            this.validator = new CreateCommunityContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(communityContract => communityContract.Name, (string)null, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            this.validator.ShouldHaveValidationErrorFor(communityContract => communityContract.Name, string.Empty, "Create");
        }

        [Fact(DisplayName = "Verify that throws error when Description string is null")]
        public void Should_Have_Error_When_Description_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(communityContract => communityContract.Description, (string)null, "Create");
        }
    }
}
