using System;
using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;

namespace Domain.Services.Impl.Profiles
{
    public class OfferStageProfile : StageProfile
    {
        public OfferStageProfile()
        {
            CreateMap<OfferStage, ReadedOfferStageContract>();
            //.ForMember(dest => dest.StageItems, opt => opt.MapFrom(src => src.StageItems));

            CreateMap<CreateOfferStageContract, OfferStage>()
                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(destination => destination.Seniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)));
            CreateMap<OfferStage, CreatedOfferStageContract>();
            CreateMap<UpdateOfferStageContract, OfferStage>()
                            .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(destination => destination.Seniority,
                opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)));
            CreateMap<UpdateOfferStageViewModel, UpdateOfferStageContract>();
        }
    }
}
