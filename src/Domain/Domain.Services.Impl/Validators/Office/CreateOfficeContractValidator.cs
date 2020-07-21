using Domain.Services.Contracts.Office;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Office
{
    public class CreateOfficeContractValidator : AbstractValidator<CreateOfficeContract>
    {
        public CreateOfficeContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
