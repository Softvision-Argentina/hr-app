// <copyright file="UpdateRoomContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Room
{
    using Domain.Services.Contracts.Room;
    using FluentValidation;

    public class UpdateRoomContractValidator : AbstractValidator<UpdateRoomContract>
    {
        public UpdateRoomContractValidator()
        {
            this.RuleFor(_ => _.Id).NotEmpty();
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}
