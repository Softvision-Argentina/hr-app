using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Services.Contracts.Offer;

namespace Domain.Services.Impl.Validators.Offer
{
    public class CreateOfferContractValidator : AbstractValidator<CreateOfferContract>
    {
        public CreateOfferContractValidator()
        {
            RuleFor(_ => _.OfferDate).NotEmpty();
            RuleFor(_ => _.Salary).NotEmpty();
            RuleFor(_ => _.RejectionReason).NotEmpty();
            RuleFor(_ => _.Status).NotEmpty();            
        }        
    }
}
