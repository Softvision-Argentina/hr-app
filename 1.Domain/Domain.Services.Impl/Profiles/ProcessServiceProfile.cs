using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Process;

namespace Domain.Services.Impl.Profiles
{
    public class ProcessServiceProfile : Profile
    {
        public ProcessServiceProfile()
        {
            CreateMap<Process, ReadedProcessContract>();
            CreateMap<UpdateProcessContract, Process>();
            CreateMap<CreateProcessContract, Process>();
            CreateMap<Process, CreatedProcessContract>();
        }
    }
}
