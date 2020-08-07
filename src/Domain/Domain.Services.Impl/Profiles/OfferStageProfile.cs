// <copyright file="OfferStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using System;
    using ApiServer.Contracts.Stage;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Stage;

    public class OfferStageProfile : StageProfile
    {
        public OfferStageProfile()
        {
            this.CreateMap<OfferStage, ReadedOfferStageContract>();

            this.CreateMap<CreateOfferStageContract, OfferStage>()
                .ForMember(
                    destination => destination.Status,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(
                    destination => destination.Seniority,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)));

            this.CreateMap<OfferStage, CreatedOfferStageContract>();

            this.CreateMap<UpdateOfferStageContract, OfferStage>()
                            .ForMember(
                                destination => destination.Status,
                                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(
                    destination => destination.Seniority,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)));

            this.CreateMap<UpdateOfferStageViewModel, UpdateOfferStageContract>();
        }
    }
}
