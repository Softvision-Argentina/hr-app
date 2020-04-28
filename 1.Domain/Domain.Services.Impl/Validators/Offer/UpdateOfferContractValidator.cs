using Domain.Services.Contracts.Offer;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Offer
{
    public class UpdateOfferContractValidator : AbstractValidator<UpdateOfferContract>
    {
        public UpdateOfferContractValidator()
        {
            RuleFor(_ => _.OfferDate).NotEmpty();
            RuleFor(_ => _.Salary).NotEmpty();
            RuleFor(_ => _.RejectionReason);
            RuleFor(_ => _.Status);
        }
    }
}
