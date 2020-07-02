using Domain.Services.Contracts.Process;
using Domain.Services.Impl.Validators.Candidate;
using Domain.Services.Impl.Validators.ReaddressStatus;
using Domain.Services.Impl.Validators.Stages.ClientStage;
using Domain.Services.Impl.Validators.Stages.HrStage;
using Domain.Services.Impl.Validators.Stages.OfferStage;
using Domain.Services.Impl.Validators.Stages.PreOfferStage;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Reservation
{
    public class UpdateProcessContractValidator : AbstractValidator<UpdateProcessContract>
    {
        public UpdateProcessContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_UPDATE, () =>
            {
                RuleFor(_ => _.Candidate).SetValidator(new UpdateCandidateContractValidator(), ValidatorConstants.RULESET_DEFAULT);
                RuleFor(_ => _.ClientStage).SetValidator(new UpdateClientStageContractValidator(), ValidatorConstants.RULESET_UPDATE);
                RuleFor(_ => _.OfferStage).SetValidator(new UpdateOfferStageContractValidator(), ValidatorConstants.RULESET_UPDATE);
                RuleFor(_ => _.PreOfferStage).SetValidator(new UpdatePreOfferStageContractValidator(), ValidatorConstants.RULESET_UPDATE);
                RuleFor(_ => _.HrStage).SetValidator(new UpdateHrStageContractValidator(), ValidatorConstants.RULESET_UPDATE);

                RuleFor(_ => _.HrStage.ReaddressStatus).SetValidator(new UpdateReaddressStatusValidator(), ValidatorConstants.RULESET_CREATE);
                RuleFor(_ => _.TechnicalStage.ReaddressStatus).SetValidator(new UpdateReaddressStatusValidator(), ValidatorConstants.RULESET_CREATE);
                RuleFor(_ => _.ClientStage.ReaddressStatus).SetValidator(new UpdateReaddressStatusValidator(), ValidatorConstants.RULESET_CREATE);
                RuleFor(_ => _.PreOfferStage.ReaddressStatus).SetValidator(new UpdateReaddressStatusValidator(), ValidatorConstants.RULESET_CREATE);

                RuleFor(_ => _.CurrentStage).IsInEnum();
                RuleFor(_ => _.EnglishLevel).MaximumLength(ValidationConstants.MAX_INPUT);
                RuleFor(_ => _.Seniority).IsInEnum();
                RuleFor(_ => _.Status).IsInEnum();

                RuleFor(_ => _.Profile).MaximumLength(ValidationConstants.MAX_INPUT);
                RuleFor(_ => _.RejectionReason).MaximumLength(ValidationConstants.MAX_TEXTAREA);

                RuleFor(_ => _.WantedSalary).LessThan(ValidationConstants.MAX_MONTHLY_INCOME);
                RuleFor(_ => _.ActualSalary).LessThan(ValidationConstants.MAX_MONTHLY_INCOME);
            });
        }
    }
}
