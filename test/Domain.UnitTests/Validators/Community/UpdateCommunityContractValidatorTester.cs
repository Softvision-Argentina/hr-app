// <copyright file="UpdateCommunityContractValidatorTester.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Validators.Community
{
    using Domain.Services.Impl.Validators.Community;
    using FluentValidation.TestHelper;
    using Xunit;

    public class UpdateCommunityContractValidatorTester
    {
        private readonly UpdateCommunityContractValidator validator;

        public UpdateCommunityContractValidatorTester()
        {
            this.validator = new UpdateCommunityContractValidator();
        }

        [Fact(DisplayName = "Verify that throws error when Name string is null")]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(communityContract => communityContract.Name, (string)null);
        }

        [Fact(DisplayName = "Verify that throws error when Name string is empty")]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            this.validator.ShouldHaveValidationErrorFor(communityContract => communityContract.Name, string.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is null")]
        public void Should_Have_Error_When_Description_Is_Null()
        {
            this.validator.ShouldHaveValidationErrorFor(communityContract => communityContract.Description, (string)null);
        }

        [Fact(DisplayName = "Verify that throws error when Description string is Empty")]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            this.validator.ShouldHaveValidationErrorFor(communityContract => communityContract.Description, string.Empty);
        }
    }
}
