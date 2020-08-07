// <copyright file="UpdatePreOfferStageContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Stages.PreOfferStage
{
    using Domain.Services.Contracts.Stage;
    using FluentValidation;

    public class UpdatePreOfferStageContractValidator : AbstractValidator<UpdatePreOfferStageContract>
    {
        public UpdatePreOfferStageContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.Bonus).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.Notes).MaximumLength(ValidationConstants.MAXTEXTAREA);

                this.RuleFor(_ => _.DNI)
                    .GreaterThanOrEqualTo(0)
                    .LessThan(ValidationConstants.MAXDNI);

                this.RuleFor(_ => _.HealthInsurance).IsInEnum();
                this.RuleFor(_ => _.Status).IsInEnum();
            });
        }
    }
}
