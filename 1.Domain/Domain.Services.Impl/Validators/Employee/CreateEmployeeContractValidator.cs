using Domain.Services.Contracts.Employee;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Employee
{
    public class CreateEmployeeContractValidator : AbstractValidator<CreateEmployeeContract>
    {
        public CreateEmployeeContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.LastName).NotEmpty();
            RuleFor(_ => _.EmailAddress).NotEmpty();
            RuleFor(_ => _.DNI).NotEmpty();
            RuleFor(_ => _.DNI).GreaterThan(0);
            RuleFor(_ => _.PhoneNumber).NotEmpty();
            RuleFor(_ => _.User).NotNull();
            RuleFor(_ => _.isReviewer).NotNull();
            RuleFor(_ => _.Role).NotNull();
        }
    }
}
