// <copyright file="StageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using System;
    using ApiServer.Contracts.Stage;
    using AutoMapper;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Stage;

    public class StageProfile : Profile
    {
        public StageProfile()
        {
            this.CreateMap<Stage, ReadedStageContract>();

            this.CreateMap<CreateStageContract, Stage>()
                .ForMember(
                    destination => destination.Status,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            this.CreateMap<Stage, CreatedStageContract>();

            this.CreateMap<UpdateStageContract, Stage>()
                                .ForMember(
                                    destination => destination.Status,
                                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            this.CreateMap<UpdateStageViewModel, UpdateStageContract>();
        }
    }
}
