using Domain.Services.Contracts.Role;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Role
{
    public class UpdateRoleContractValidator : AbstractValidator<UpdateRoleContract>
    {
        public UpdateRoleContractValidator()
        {
            RuleFor(_ => _.Id).NotEmpty();
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.isActive).NotNull();
        }
    }
}
