using Domain.Services.Contracts.Seed;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Seed
{
    public class UpdateDummyContractValidator: AbstractValidator<UpdateDummyContract>
    {
        public UpdateDummyContractValidator()
        {
            RuleFor(_ => _.TestValue).NotEmpty();
        }
    }
}
