using Domain.Services.Contracts.ReaddressStatus;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Impl.Validators.ReaddressStatus
{
    public class CreateReaddressStatusValidator:  AbstractValidator<CreateReaddressStatus>
    {
        public CreateReaddressStatusValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Feedback).MaximumLength(ValidationConstants.MAX_TEXTAREA);
                RuleFor(_ => _.FromStatus).IsInEnum();
                RuleFor(_ => _.ToStatus).IsInEnum();
            });
        }
    }
}
