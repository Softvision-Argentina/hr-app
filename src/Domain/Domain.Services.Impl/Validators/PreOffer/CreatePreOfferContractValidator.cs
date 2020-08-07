// <copyright file="CreatePreOfferContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.PreOffer
{
    using Domain.Services.Contracts.PreOffer;
    using FluentValidation;

    public class CreatePreOfferContractValidator : AbstractValidator<CreatePreOfferContract>
    {
        public CreatePreOfferContractValidator()
        {
            this.RuleFor(_ => _.PreOfferDate).NotEmpty();
            this.RuleFor(_ => _.Salary).NotEmpty();
            this.RuleFor(_ => _.VacationDays).NotEmpty();
            this.RuleFor(_ => _.HealthInsurance).NotEmpty();
            this.RuleFor(_ => _.Status).NotEmpty();
        }
    }
}
