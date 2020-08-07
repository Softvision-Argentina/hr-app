// <copyright file="UpdateProcessContractValidator.cs" company="Softvision">
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
    using FluentValidation;

    public class UpdateProcessContractValidator : AbstractValidator<UpdateProcessContract>
    {
        public UpdateProcessContractValidator()
        {
            this.RuleSet(ValidatorConstants.RULESETUPDATE, () =>
            {
                this.RuleFor(_ => _.Candidate).SetValidator(new UpdateCandidateContractValidator(), ValidatorConstants.RULESETDEFAULT);
                this.RuleFor(_ => _.ClientStage).SetValidator(new UpdateClientStageContractValidator(), ValidatorConstants.RULESETUPDATE);
                this.RuleFor(_ => _.OfferStage).SetValidator(new UpdateOfferStageContractValidator(), ValidatorConstants.RULESETUPDATE);
                this.RuleFor(_ => _.PreOfferStage).SetValidator(new UpdatePreOfferStageContractValidator(), ValidatorConstants.RULESETUPDATE);
                this.RuleFor(_ => _.HrStage).SetValidator(new UpdateHrStageContractValidator(), ValidatorConstants.RULESETUPDATE);

                this.RuleFor(_ => _.HrStage.ReaddressStatus).SetValidator(new UpdateReaddressStatusValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.TechnicalStage.ReaddressStatus).SetValidator(new UpdateReaddressStatusValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.ClientStage.ReaddressStatus).SetValidator(new UpdateReaddressStatusValidator(), ValidatorConstants.RULESETCREATE);
                this.RuleFor(_ => _.PreOfferStage.ReaddressStatus).SetValidator(new UpdateReaddressStatusValidator(), ValidatorConstants.RULESETCREATE);

                this.RuleFor(_ => _.CurrentStage).IsInEnum();
                this.RuleFor(_ => _.EnglishLevel).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.Seniority).IsInEnum();
                this.RuleFor(_ => _.Status).IsInEnum();

                this.RuleFor(_ => _.Profile).MaximumLength(ValidationConstants.MAXINPUT);
                this.RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAXTEXTAREA);

                this.RuleFor(_ => _.WantedSalary).LessThan(ValidationConstants.MAXMONTHLYINCOME);
                this.RuleFor(_ => _.ActualSalary).LessThan(ValidationConstants.MAXMONTHLYINCOME);
            });
        }
    }
}
