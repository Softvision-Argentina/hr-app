// <copyright file="UpdateOfferStageContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Stages.OfferStage
{
    using Domain.Services.Contracts.Stage;
    using FluentValidation;

    public class UpdateOfferStageContractValidator : AbstractValidator<UpdateOfferStageContract>
    {
        public UpdateOfferStageContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.Bonus).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.Notes).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAXTEXTAREA);

                this.RuleFor(_ => _.HealthInsurance).IsInEnum();
                this.RuleFor(_ => _.Status).IsInEnum();
                this.RuleFor(_ => _.Seniority).IsInEnum();
            });
        }
    }
}
