using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;
using System;

namespace Domain.Services.Impl.Profiles
{
    public class StageProfile : Profile
    {
        public StageProfile()
        {
            CreateMap<Stage, ReadedStageContract>();

            CreateMap<CreateStageContract, Stage>()
                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            CreateMap<Stage, CreatedStageContract>();

            CreateMap<UpdateStageContract, Stage>()
                                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            CreateMap<UpdateStageViewModel, UpdateStageContract>();
        }
    }
}
