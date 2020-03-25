using Domain.Services.Contracts.Task;
using Domain.Services.Impl.Validators.Task;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Domain.Services.Tests.Impl.Validators.Task
{
    public class CreateTaskContractValidatorTest
    {
        private readonly CreateTaskContractValidator validator;

        public CreateTaskContractValidatorTest()
        {
            validator = new CreateTaskContractValidator();
        }

        [Fact(DisplayName = "Verify that does not throw error when data is valid")]
        public void Should_WorkFine_When_DataIsValid()
        {
            var createTask = new CreateTaskContract()
            {
                Title = "test tittle",
                IsApprove = true,
                ConsultantId = 1
            };

            validator.ValidateAndThrow(createTask);

            //It should work fine therefore it should not throw error
        }

        [Fact(DisplayName = "Verify that throws error when Title is Empty")]
        public void Should_ThrowError_When_TitleIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(createTaskContract => createTaskContract.Title, string.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when ConsultantId is Empty")]
        public void Should_ThrowError_When_ConsultantIdIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(createTaskContract => createTaskContract.ConsultantId, default(int));
        }
    }
}
