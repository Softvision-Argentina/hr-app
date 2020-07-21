using Domain.Services.Contracts.PreOffer;
using FluentValidation;

namespace Domain.Services.Impl.Validators.PreOffer
{
    public class UpdatePreOfferContractValidator : AbstractValidator<UpdatePreOfferContract>
    {
        public UpdatePreOfferContractValidator()
        {
            RuleFor(_ => _.PreOfferDate).NotEmpty();
            RuleFor(_ => _.Salary).NotEmpty();
            RuleFor(_ => _.VacationDays).NotEmpty();
            RuleFor(_ => _.HealthInsurance).NotEmpty();
            RuleFor(_ => _.Status);
        }
    }
}
