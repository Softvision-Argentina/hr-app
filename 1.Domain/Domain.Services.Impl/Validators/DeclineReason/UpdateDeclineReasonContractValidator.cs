using Domain.Services.Contracts;
using FluentValidation;

namespace Domain.Services.Impl.Validators
{
    public class UpdateDeclineReasonContractValidator : AbstractValidator<UpdateDeclineReasonContract>
    {
        public UpdateDeclineReasonContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
