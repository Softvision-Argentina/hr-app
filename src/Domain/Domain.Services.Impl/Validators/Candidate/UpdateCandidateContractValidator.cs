using Domain.Services.Contracts.Candidate;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Candidate
{
    public class UpdateCandidateContractValidator : AbstractValidator<UpdateCandidateContract>
    {
        public UpdateCandidateContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty()
                .MaximumLength(ValidationConstants.MAX_INPUT);

            RuleFor(_ => _.LastName).NotEmpty()
                .MaximumLength(ValidationConstants.MAX_INPUT);

            //RuleFor(_ => _.User).NotEmpty();
            RuleFor(_ => _.Community).NotEmpty();
            //RuleFor(_ => _.Profile).NotEmpty();

            RuleFor(_ => _.Cv)
                .MaximumLength(100);

            RuleFor(_ => _.EmailAddress)
                .MaximumLength(ValidationConstants.MAX_INPUT_EMAIL);

            RuleFor(_ => _.EnglishLevel).IsInEnum();

            RuleFor(_ => _.KnownFrom)
                .MaximumLength(ValidationConstants.MAX_INPUT);

            RuleFor(_ => _.LinkedInProfile)
                .MaximumLength(ValidationConstants.MAX_INPUT);

            RuleFor(_ => _.PhoneNumber)
                .MaximumLength(ValidationConstants.MAX_PHONE_NUMBER);

            RuleFor(_ => _.ReferredBy)
                .MaximumLength(ValidationConstants.MAX_INPUT);

            RuleFor(_ => _.Status).IsInEnum();

            RuleFor(_ => _.DNI).LessThan(ValidationConstants.MAX_DNI);
        }
    }
}
