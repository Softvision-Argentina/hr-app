// <copyright file="UpdateTaskContractValidatorTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Validators.Task
{
    using Domain.Services.Contracts.Task;
    using Domain.Services.Impl.Validators.Task;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using Xunit;

    public class UpdateTaskContractValidatorTest
    {
        private readonly UpdateTaskContractValidator validator;

        public UpdateTaskContractValidatorTest()
        {
            this.validator = new UpdateTaskContractValidator();
        }

        [Fact(DisplayName = "Verify that does not throw error when data is valid")]
        public void Should_WorkFine_When_DataIsValid()
        {
            var updateTask = new UpdateTaskContract()
            {
                Title = "test tittle",
                IsApprove = true,
                UserId = 1,
            };

            this.validator.ValidateAndThrow(updateTask);

            // It should work fine therefore it should not throw error
        }

        [Fact(DisplayName = "Verify that throws error when Title is Empty")]
        public void Should_ThrowError_When_TitleIsEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(updateTaskContract => updateTaskContract.Title, string.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when UserId is Empty")]
        public void Should_ThrowError_When_UserIdIsEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(updateTaskContract => updateTaskContract.UserId, default(int));
        }
    }
}
