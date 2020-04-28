using Domain.Services.Contracts.Community;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Community
{
    public class CreateCommunityContractValidator : AbstractValidator<CreateCommunityContract>
    {
        public CreateCommunityContractValidator()
        {
            RuleSet(ValidatorConstants.RULESET_CREATE, () =>
            {
                RuleFor(_ => _.Name).NotEmpty();
                RuleFor(_ => _.Description).NotNull();
            });
        }
    }
}
