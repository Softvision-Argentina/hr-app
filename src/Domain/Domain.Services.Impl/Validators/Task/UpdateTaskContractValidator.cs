using Domain.Services.Contracts.Task;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Task
{
    public class UpdateTaskContractValidator : AbstractValidator<UpdateTaskContract>
    {
        public UpdateTaskContractValidator()
        {
            RuleFor(_ => _.Title).NotEmpty();
            RuleFor(_ => _.IsApprove).NotNull();
            RuleFor(_ => _.UserId).NotEmpty();
        }
    }
}
