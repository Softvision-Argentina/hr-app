using Domain.Services.Contracts.CompanyCalendar;
using FluentValidation;

namespace Domain.Services.Impl.Validators.CompanyCalendar
{
    public class CreateCompanyCalendarContractValidator : AbstractValidator<CreateCompanyCalendarContract>
    {
        public CreateCompanyCalendarContractValidator()
        {
            RuleFor(_ => _.Type).NotEmpty();
            RuleFor(_ => _.Date).NotEmpty();
        }
    }
}
