// <copyright file="UpdateDaysOffContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.DaysOff
{
    using Domain.Services.Contracts.DaysOff;
    using FluentValidation;

    public class UpdateDaysOffContractValidator : AbstractValidator<UpdateDaysOffContract>
    {
        public UpdateDaysOffContractValidator()
        {
            this.RuleFor(_ => _.Type).NotNull().WithMessage("Type must not be empty");
            this.RuleFor(_ => _.Date).NotEmpty();
            this.RuleFor(_ => _.EndDate).NotEmpty();
        }
    }
}
