// <copyright file="CreateReservationContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Reservation
{
    using Domain.Services.Contracts.Reservation;
    using FluentValidation;

    public class CreateReservationContractValidator : AbstractValidator<CreateReservationContract>
    {
        public CreateReservationContractValidator()
        {
            this.RuleFor(_ => _.SinceReservation).NotNull();
            this.RuleFor(_ => _.UntilReservation).NotNull();
            this.RuleFor(_ => _.RoomId).NotNull();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
