using Domain.Services.Contracts.CompanyCalendar;
using FluentValidation;

namespace Domain.Services.Impl.Validators.CompanyCalendar
{
    public class UpdateCompanyCalendarContractValidator : AbstractValidator<UpdateCompanyCalendarContract>
    {
        public UpdateCompanyCalendarContractValidator()
        {
            RuleFor(_ => _.Type).NotEmpty();
            RuleFor(_ => _.Date).NotEmpty();
        }
    }
}
