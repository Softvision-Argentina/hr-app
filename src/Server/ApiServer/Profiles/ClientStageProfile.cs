// <copyright file="ClientStageProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Stage;
    using AutoMapper;
    using Domain.Services.Contracts.Stage;
    using Domain.Services.Contracts.Stage.ClientStage;

    public class ClientStageProfile : Profile
    {
        public ClientStageProfile()
        {
            this.CreateMap<CreateClientStageViewModel, CreateClientStageContract>();
            this.CreateMap<CreatedClientStageContract, CreatedClientStageViewModel>();
            this.CreateMap<ReadedClientStageContract, ReadedClientStageViewModel>();
            this.CreateMap<UpdateClientStageViewModel, UpdateClientStageContract>();
        }
    }
}
