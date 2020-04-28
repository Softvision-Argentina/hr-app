using ApiServer.Contracts.Stage;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;
using Domain.Services.Contracts.Stage.ClientStage;
using System;

namespace Domain.Services.Impl.Profiles
{
    public class ClientStageProfile : StageProfile
    {
        public ClientStageProfile()
        {
            CreateMap<ClientStage, ReadedClientStageContract>();

            CreateMap<CreateClientStageContract, ClientStage>()
                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            CreateMap<ClientStage, CreatedClientStageContract>();

            CreateMap<UpdateClientStageContract, ClientStage>()
                            .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            CreateMap<UpdateClientStageViewModel, UpdateClientStageContract>();
        }
    }
}
