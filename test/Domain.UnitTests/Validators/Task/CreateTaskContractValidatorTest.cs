﻿// <copyright file="CreateTaskContractValidatorTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Validators.Task
{
    using Domain.Services.Contracts.Task;
    using Domain.Services.Impl.Validators.Task;
    using FluentValidation;
    using FluentValidation.TestHelper;
    using Xunit;

    public class CreateTaskContractValidatorTest
    {
        private readonly CreateTaskContractValidator validator;

        public CreateTaskContractValidatorTest()
        {
            this.validator = new CreateTaskContractValidator();
        }

        [Fact(DisplayName = "Verify that does not throw error when data is valid")]
        public void Should_WorkFine_When_DataIsValid()
        {
            var createTask = new CreateTaskContract()
            {
                Title = "test tittle",
                IsApprove = true,
                UserId = 1,
            };

            this.validator.ValidateAndThrow(createTask);

            // It should work fine therefore it should not throw error
        }

        [Fact(DisplayName = "Verify that throws error when Title is Empty")]
        public void Should_ThrowError_When_TitleIsEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(createTaskContract => createTaskContract.Title, string.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when UserId is Empty")]
        public void Should_ThrowError_When_UserIdIsEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(createTaskContract => createTaskContract.UserId, default(int));
        }
    }
}
