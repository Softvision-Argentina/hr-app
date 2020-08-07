// <copyright file="ProcessStatusContractValidator.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Validators.Stage
{
    using Domain.Services.Contracts.Process;
    using FluentValidation;

    public class ProcessStatusContractValidator : AbstractValidator<ReadedProcessContract>
    {
        public ProcessStatusContractValidator()
        {
            // TODO: Change messages to English.
            this.RuleFor(_ => _.Status).NotEmpty()
                                  .WithMessage("El estado del proceso es obligatorio");
            this.RuleFor(_ => _.Status).NotEqual(Model.Enum.ProcessStatus.Hired)
                                  .WithMessage("No es posible editar una etapa de un proceso finalizado.");
        }
    }
}
