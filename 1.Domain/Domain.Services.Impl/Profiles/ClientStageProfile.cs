using System;
using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;
using Domain.Services.Contracts.Stage.StageItem;

namespace Domain.Services.Impl.Profiles
{
    public class ClientStageProfile : StageProfile
    {
        public ClientStageProfile()
        {
            CreateMap<ClientStage, ReadedClientStageContract>();
            //.ForMember(dest => dest.StageItems, opt => opt.MapFrom(src => src.StageItems));

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
