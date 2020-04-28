using ApiServer.Contracts.Stage;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;
using System;

namespace Domain.Services.Impl.Profiles
{
    public class OfferStageProfile : StageProfile
    {
        public OfferStageProfile()
        {
            CreateMap<OfferStage, ReadedOfferStageContract>();

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
