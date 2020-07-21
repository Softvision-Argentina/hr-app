using Domain.Services.Contracts.Process;
using FluentValidation;

namespace Domain.Services.Impl.Validators.Stage
{
    public class ProcessStatusContractValidator : AbstractValidator<ReadedProcessContract>
    {
        public ProcessStatusContractValidator()
        {
            // TODO: Change messages to English.
            RuleFor(_ => _.Status).NotEmpty()
                                  .WithMessage("El estado del proceso es obligatorio");
            RuleFor(_ => _.Status).NotEqual(Model.Enum.ProcessStatus.Hired)
                                  .WithMessage("No es posible editar una etapa de un proceso finalizado.");
        }
    }
}
