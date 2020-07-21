using Domain.Services.Contracts.EmployeeCasualty;
using FluentValidation;

namespace Domain.Services.Impl.Validators.EmployeeCasualty
{
    public class UpdateEmployeeCasualtyContractValidator : AbstractValidator<UpdateEmployeeCasualtyContract>
    {
        public UpdateEmployeeCasualtyContractValidator()
        {
            RuleFor(_ => _.Month).NotEmpty();
            RuleFor(_ => _.Year).NotEmpty();
            RuleFor(_ => _.Value).NotEmpty();
        }
    }
}
