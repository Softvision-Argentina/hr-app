using Domain.Services.Contracts.Reservation;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Reservation
{
    public class CreateReservationContractValidator : AbstractValidator<CreateReservationContract>
    {
        public CreateReservationContractValidator()
        {
            RuleFor(_ => _.SinceReservation).NotNull();
            RuleFor(_ => _.UntilReservation).NotNull();
            RuleFor(_ => _.RoomId).NotNull();
            RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
