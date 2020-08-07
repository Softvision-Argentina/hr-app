// <copyright file="CreateClientStageContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Stages.ClientStage
{
    using Domain.Services.Contracts.Stage;
    using FluentValidation;

    public class CreateClientStageContractValidator : AbstractValidator<CreateClientStageContract>
    {
        public CreateClientStageContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETCREATE, () =>
            {
                this.RuleFor(_ => _.DelegateName).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.Interviewer).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAXTEXTAREA);

                this.RuleFor(_ => _.Status).IsInEnum();
            });
        }
    }
}
