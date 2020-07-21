using Domain.Services.Contracts.Stage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stage
{
    public class CreateStageContractValidator : AbstractValidator<CreateStageContract>
    {
        public CreateStageContractValidator()
        {
            RuleFor(_ => _.Status).NotEmpty();
        }
    }
}
