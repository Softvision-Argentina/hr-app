using Domain.Services.Contracts.PreOffer;
using FluentValidation;

namespace Domain.Services.Impl.Validators.PreOffer
{
    public class CreatePreOfferContractValidator : AbstractValidator<CreatePreOfferContract>
    {
        public CreatePreOfferContractValidator()
        {
            RuleFor(_ => _.PreOfferDate).NotEmpty();
            RuleFor(_ => _.Salary).NotEmpty();
            RuleFor(_ => _.VacationDays).NotEmpty();
            RuleFor(_ => _.HealthInsurance).NotEmpty();
            RuleFor(_ => _.Status).NotEmpty();            
        }        
    }
}
