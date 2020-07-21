using Domain.Services.Contracts.Skill;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Skill
{
    public class CreateSkillContractValidator : AbstractValidator<CreateSkillContract>
    {
        public CreateSkillContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Type).NotEmpty();
        }
    }
}
