using Domain.Services.Contracts.SkillType;
using FluentValidation;

namespace Domain.Services.Impl.Validators.SkillType
{
    public class CreateSkillTypeContractValidator : AbstractValidator<CreateSkillTypeContract>
    {
        public CreateSkillTypeContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
