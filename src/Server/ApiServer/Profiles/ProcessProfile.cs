using ApiServer.Contracts.Process;
using AutoMapper;
using Domain.Services.Contracts.Process;

namespace ApiServer.Profiles
{
    public class ProcessProfile : Profile
    {
        public ProcessProfile()
        {
            CreateMap<ReadedProcessContract, ReadedProcessViewModel>();
            CreateMap<CreateProcessViewModel, CreateProcessContract>();
            CreateMap<CreatedProcessContract, CreatedProcessViewModel>();
            CreateMap<UpdateProcessViewModel, UpdateProcessContract>();
        }
    }
}
