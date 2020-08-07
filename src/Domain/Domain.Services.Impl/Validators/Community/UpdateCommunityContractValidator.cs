// <copyright file="UpdateCommunityContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Community
{
    using Domain.Services.Contracts.Community;
    using FluentValidation;

    public class UpdateCommunityContractValidator : AbstractValidator<UpdateCommunityContract>
    {
        public UpdateCommunityContractValidator()
        {
            this.RuleFor(_ => _.Name).NotEmpty();
            this.RuleFor(_ => _.Description).NotEmpty();
        }
    }
}