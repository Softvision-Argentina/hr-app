// <copyright file="UpdateTechnicalStageContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Stages.TechnicalStage
{
    using Domain.Services.Contracts.Stage;
    using FluentValidation;

    public class UpdateTechnicalStageContractValidator : AbstractValidator<UpdateTechnicalStageContract>
    {
        public UpdateTechnicalStageContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.AlternativeSeniority).IsInEnum();
                this.RuleFor(_ => _.EnglishLevel).IsInEnum();
                this.RuleFor(_ => _.Seniority).IsInEnum();
                this.RuleFor(_ => _.Status).IsInEnum();

                this.RuleFor(_ => _.Client).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAXTEXTAREA);
            });
        }
    }
}
