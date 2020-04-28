using Domain.Services.Contracts.Room;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Room
{
    public class CreateRoomContractValidator : AbstractValidator<CreateRoomContract>
    {
        public CreateRoomContractValidator()
        {
            RuleFor(_ => _.Name).NotEmpty();
            RuleFor(_ => _.Description).NotEmpty();
            RuleFor(_ => _.OfficeId).NotNull();
        }
    }
}
