using Domain.Services.Contracts.ReaddressStatus;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Impl.Validators.ReaddressStatus
{
    public class UpdateReaddressStatusValidator:  AbstractValidator<UpdateReaddressStatus>
    {
        public UpdateReaddressStatusValidator()
        {
            RuleSet(ValidatorConstants.RULESET_UPDATE, () =>
            {
                RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.FromStatus).IsInEnum();
                RuleFor(_ => _.ToStatus).IsInEnum();
            });
        }
    }
}
