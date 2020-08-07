// <copyright file="PreOfferStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using System;
    using ApiServer.Contracts.Stage;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Stage;

    public class PreOfferStageProfile : StageProfile
    {
        public PreOfferStageProfile()
        {
            this.CreateMap<PreOfferStage, ReadedPreOfferStageContract>();

            this.CreateMap<CreatePreOfferStageContract, PreOfferStage>()
                .ForMember(
                    destination => destination.Status,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            this.CreateMap<PreOfferStage, CreatedPreOfferStageContract>();

            this.CreateMap<UpdatePreOfferStageContract, PreOfferStage>()
                            .ForMember(
                                destination => destination.Status,
                                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            this.CreateMap<UpdatePreOfferStageViewModel, UpdatePreOfferStageContract>();
        }
    }
}
