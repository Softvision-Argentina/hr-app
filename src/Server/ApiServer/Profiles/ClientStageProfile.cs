using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Services.Contracts.Stage;
using Domain.Services.Contracts.Stage.ClientStage;

namespace ApiServer.Profiles
{
    public class ClientStageProfile : Profile
    {
        public ClientStageProfile()
        {
            CreateMap<CreateClientStageViewModel, CreateClientStageContract>();
            CreateMap<CreatedClientStageContract, CreatedClientStageViewModel>();
            CreateMap<ReadedClientStageContract, ReadedClientStageViewModel>();
            CreateMap<UpdateClientStageViewModel, UpdateClientStageContract>();
        }
    }
}
