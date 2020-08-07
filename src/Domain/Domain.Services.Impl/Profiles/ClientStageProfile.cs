// <copyright file="ClientStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using System;
    using ApiServer.Contracts.Stage;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Stage;
    using Domain.Services.Contracts.Stage.ClientStage;

    public class ClientStageProfile : StageProfile
    {
        public ClientStageProfile()
        {
            this.CreateMap<ClientStage, ReadedClientStageContract>();

            this.CreateMap<CreateClientStageContract, ClientStage>()
                .ForMember(
                    destination => destination.Status,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            this.CreateMap<ClientStage, CreatedClientStageContract>();

            this.CreateMap<UpdateClientStageContract, ClientStage>()
                            .ForMember(
                                destination => destination.Status,
                                opt => opt.MapFrom(source => Enum.GetName(typeof(StageStatus), source.Status)));

            this.CreateMap<UpdateClientStageViewModel, UpdateClientStageContract>();
        }
    }
}
