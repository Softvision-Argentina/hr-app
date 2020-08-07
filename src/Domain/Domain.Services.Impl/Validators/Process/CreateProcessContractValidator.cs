// <copyright file="CreateProcessContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Reservation
{
    using Domain.Services.Contracts.Process;
    using Domain.Services.Impl.Validators.Candidate;
    using Domain.Services.Impl.Validators.ReaddressStatus;
    using Domain.Services.Impl.Validators.Stages.ClientStage;
    using Domain.Services.Impl.Validators.Stages.HrStage;
    using Domain.Services.Impl.Validators.Stages.OfferStage;
    using Domain.Services.Impl.Validators.Stages.PreOfferStage;
    using Domain.Services.Impl.Validators.Stages.TechnicalStage;
    using FluentValidation;

    public class CreateProcessContractValidator : AbstractValidator<CreateProcessContract>
    {
        public CreateProcessContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETCREATE, () =>
            {
                this.RuleFor(_ => _.Candidate).SetValidator(new UpdateCandidateContractValidator(), ValidatorConstants.RULESETDEFAULT);

                this.RuleFor(_ => _.HrStage).SetValidator(new CreateHrStageContractValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.TechnicalStage).SetValidator(new CreateTechnicalStageContractValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.ClientStage).SetValidator(new CreateClientStageContractValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.PreOfferStage).SetValidator(new CreatePreOfferStageContractValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.OfferStage).SetValidator(new CreateOfferStageContractValidator(), ValidatorConstants.RULESETCREATE);

                this.RuleFor(_ => _.HrStage.ReaddressStatus).SetValidator(new CreateReaddressStatusValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.TechnicalStage.ReaddressStatus).SetValidator(new CreateReaddressStatusValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.ClientStage.ReaddressStatus).SetValidator(new CreateReaddressStatusValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.PreOfferStage.ReaddressStatus).SetValidator(new CreateReaddressStatusValidator(), ValidatorConstants.RULESETCREATE);

                this.RuleFor(_ => _.CurrentStage).IsInEnum();
                this.RuleFor(_ => _.Seniority).IsInEnum();
                this.RuleFor(_ => _.Status).IsInEnum();

                this.RuleFor(_ => _.EnglishLevel).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.Profile).MaximumLength(ValidationConstants.MAXTEXTAREA);
                this.RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAXTEXTAREA);

                this.RuleFor(_ => _.WantedSalary).LessThan(ValidationConstants.MAXMONTHLYINCOME);
                this.RuleFor(_ => _.ActualSalary).LessThan(ValidationConstants.MAXMONTHLYINCOME);
            });
        }
    }
}
