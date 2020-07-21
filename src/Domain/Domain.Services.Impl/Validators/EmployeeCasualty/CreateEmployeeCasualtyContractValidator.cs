using Domain.Services.Contracts.EmployeeCasualty;
using FluentValidation;

namespace Domain.Services.Impl.Validators.EmployeeCasualty
{
    public class CreateEmployeeCasualtyContractValidator : AbstractValidator<CreateEmployeeCasualtyContract>
    {
        public CreateEmployeeCasualtyContractValidator()
        {
            RuleFor(_ => _.Month).NotEmpty();
            RuleFor(_ => _.Year).NotEmpty();
            RuleFor(_ => _.Value).NotEmpty();
        }
    }
}
