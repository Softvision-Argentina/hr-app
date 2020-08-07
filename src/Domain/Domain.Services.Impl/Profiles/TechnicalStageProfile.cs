// <copyright file="TechnicalStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using System;
    using ApiServer.Contracts.Stage;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Stage;

    public class TechnicalStageProfile : StageProfile
    {
        public TechnicalStageProfile()
        {
            this.CreateMap<TechnicalStage, ReadedTechnicalStageContract>();

            this.CreateMap<CreateTechnicalStageContract, TechnicalStage>()
                .ForMember(
                    destination => destination.Status,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(
                    destination => destination.Seniority,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)))
                .ForMember(
                    destination => destination.AlternativeSeniority,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.AlternativeSeniority)))
                .ForMember(
                    destination => destination.EnglishLevel,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)));

            this.CreateMap<TechnicalStage, CreatedTechnicalStageContract>();

            this.CreateMap<UpdateTechnicalStageContract, TechnicalStage>()
                                .ForMember(
                                    destination => destination.Status,
                                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)))
                .ForMember(
                    destination => destination.Seniority,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.Seniority)))
                .ForMember(
                    destination => destination.AlternativeSeniority,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(Seniority), source.AlternativeSeniority)))
                .ForMember(
                    destination => destination.EnglishLevel,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)));

            this.CreateMap<UpdateTechnicalStageViewModel, UpdateTechnicalStageContract>();
        }
    }
}
