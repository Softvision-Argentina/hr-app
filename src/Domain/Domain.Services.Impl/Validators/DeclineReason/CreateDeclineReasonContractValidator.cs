using Domain.Services.Contracts;
using FluentValidation;

namespace Domain.Services.Impl.Validators
{
    public class CreateDeclineReasonContractValidator : AbstractValidator<CreateDeclineReasonContract>
    {
        public CreateDeclineReasonContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
