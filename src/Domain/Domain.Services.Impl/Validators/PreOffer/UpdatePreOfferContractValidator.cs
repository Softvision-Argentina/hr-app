// <copyright file="UpdatePreOfferContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.PreOffer
{
    using Domain.Services.Contracts.PreOffer;
    using FluentValidation;

    public class UpdatePreOfferContractValidator : AbstractValidator<UpdatePreOfferContract>
    {
        public UpdatePreOfferContractValidator()
        {
            this.RuleFor(_ => _.PreOfferDate).NotEmpty();
            this.RuleFor(_ => _.Salary).NotEmpty();
            this.RuleFor(_ => _.VacationDays).NotEmpty();
            this.RuleFor(_ => _.HealthInsurance).NotEmpty();
            this.RuleFor(_ => _.Status);
        }
    }
}
