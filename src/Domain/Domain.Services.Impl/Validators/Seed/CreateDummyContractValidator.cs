using Domain.Services.Contracts.Seed;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Seed
{
    public class CreateDummyContractValidator: AbstractValidator<CreateDummyContract>
    {
        public CreateDummyContractValidator()
        {
            RuleFor(_ => _.TestValue).NotEmpty();
        }
    }
}
