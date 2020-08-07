// <copyright file="CreateRoomContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Room
{
    using Domain.Services.Contracts.Room;
    using FluentValidation;

    public class CreateRoomContractValidator : AbstractValidator<CreateRoomContract>
    {
        public CreateRoomContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
            this.RuleFor(_ => _.OfficeId).NotNull();
        }
    }
}
