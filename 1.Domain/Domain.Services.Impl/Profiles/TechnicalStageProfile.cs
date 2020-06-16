using ApiServer.Contracts.Stage;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;
using System;

namespace Domain.Services.Impl.Profiles
{
    public class TechnicalStageProfile : StageProfile
    {
        public TechnicalStageProfile()
        {
            CreateMap<TechnicalStage, ReadedTechnicalStageContract>();

            CreateMap<CreateTechnicalStageContract, TechnicalStage>()
                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(destination => destination.Seniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)))
                .ForMember(destination => destination.AlternativeSeniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.AlternativeSeniority)))
                .ForMember(destination => destination.EnglishLevel,
                opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)));

            CreateMap<TechnicalStage, CreatedTechnicalStageContract>();

            CreateMap<UpdateTechnicalStageContract, TechnicalStage>()
                                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(destination => destination.Seniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)))
                .ForMember(destination => destination.AlternativeSeniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.AlternativeSeniority)))
                .ForMember(destination => destination.EnglishLevel,
                opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)));

            CreateMap<UpdateTechnicalStageViewModel, UpdateTechnicalStageContract>();
        }
    }
}
