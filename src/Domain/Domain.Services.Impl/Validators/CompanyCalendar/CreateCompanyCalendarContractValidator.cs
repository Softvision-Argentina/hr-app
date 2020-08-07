// <copyright file="CreateCompanyCalendarContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.CompanyCalendar
{
    using Domain.Services.Contracts.CompanyCalendar;
    using FluentValidation;

    public class CreateCompanyCalendarContractValidator : AbstractValidator<CreateCompanyCalendarContract>
    {
        public CreateCompanyCalendarContractValidator()
        {
            this.RuleFor(_ => _.Type).NotEmpty();
            this.RuleFor(_ => _.Date).NotEmpty();
        }
    }
}
