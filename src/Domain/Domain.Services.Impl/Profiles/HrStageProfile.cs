// <copyright file="HrStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using System;
    using ApiServer.Contracts.Stage;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Stage;

    public class HrStageProfile : StageProfile
    {
        public HrStageProfile()
        {
            this.CreateMap<HrStage, ReadedHrStageContract>();

            this.CreateMap<CreateHrStageContract, HrStage>()
                .ForMember(
                    destination => destination.Status,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(
                    destination => destination.EnglishLevel,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)))
                .ForMember(
                    destination => destination.RejectionReasonsHr,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(RejectionReasonsHr), source.RejectionReasonsHr)));

            this.CreateMap<HrStage, CreatedHrStageContract>();

            this.CreateMap<UpdateHrStageContract, HrStage>()
                .ForMember(
                    destination => destination.Status,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(
                    destination => destination.EnglishLevel,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)))
                 .ForMember(
                     destination => destination.RejectionReasonsHr,
                     opt => opt.MapFrom(source => Enum.GetName(typeof(RejectionReasonsHr), source.RejectionReasonsHr)));

            this.CreateMap<UpdateHrStageViewModel, UpdateHrStageContract>();
        }
    }
}
