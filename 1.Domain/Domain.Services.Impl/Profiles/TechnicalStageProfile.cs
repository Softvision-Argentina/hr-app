using System;
using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;

namespace Domain.Services.Impl.Profiles
{
    public class TechnicalStageProfile : StageProfile
    {
        public TechnicalStageProfile()
        {
            CreateMap<TechnicalStage, ReadedTechnicalStageContract>();
            //.ForMember(dest => dest.TechnicalStageItems, opt => opt.MapFrom(src => src.TechnicalStageItems));

            CreateMap<CreateTechnicalStageContract, TechnicalStage>()
                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(destination => destination.Seniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)))
                .ForMember(destination => destination.AlternativeSeniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.AlternativeSeniority)));
            CreateMap<TechnicalStage, CreatedTechnicalStageContract>();
            CreateMap<UpdateTechnicalStageContract, TechnicalStage>()
                                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(destination => destination.Seniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)))
                .ForMember(destination => destination.AlternativeSeniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.AlternativeSeniority)));
            CreateMap<UpdateTechnicalStageViewModel, UpdateTechnicalStageContract>();
        }
    }
}
