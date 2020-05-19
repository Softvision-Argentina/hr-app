using ApiServer.Contracts.Stage;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Stage;
using System;

namespace Domain.Services.Impl.Profiles
{
    public class PreOfferStageProfile : StageProfile
    {
        public PreOfferStageProfile()
        {
            CreateMap<PreOfferStage, ReadedPreOfferStageContract>();

            CreateMap<CreatePreOfferStageContract, PreOfferStage>()
                .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            CreateMap<PreOfferStage, CreatedPreOfferStageContract>();

            CreateMap<UpdatePreOfferStageContract, PreOfferStage>()
                            .ForMember(destination => destination.Status,
                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            CreateMap<UpdatePreOfferStageViewModel, UpdatePreOfferStageContract>();
        }
    }
}
