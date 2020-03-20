using Domain.Services.Contracts.Task;
using Domain.Services.Impl.Validators.Task;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Impl.Validators.Task
{

    public class UpdateTaskContractValidatorTest
    {
        private UpdateTaskContractValidator validator;

        public UpdateTaskContractValidatorTest()
        {
            validator = new UpdateTaskContractValidator();
        }

        [Fact(DisplayName = "Verify that does not throw error when data is valid")]
        public void Should_WorkFine_When_DataIsValid()
        {
            var updateTask = new UpdateTaskContract()
            {
                Title = "test tittle",
                IsApprove = true,
                ConsultantId = 1
            };

            validator.ValidateAndThrow(updateTask);

            //It should work fine therefore it should not throw error
        }

        [Fact(DisplayName = "Verify that throws error when Title is Empty")]
        public void Should_ThrowError_When_TitleIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(updateTaskContract => updateTaskContract.Title, string.Empty);
        }

        [Fact(DisplayName = "Verify that throws error when ConsultantId is Empty")]
        public void Should_ThrowError_When_ConsultantIdIsEmpty()
        {
            validator.ShouldHaveValidationErrorFor(updateTaskContract => updateTaskContract.ConsultantId, default(int) );
        }
    }
}
