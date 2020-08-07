// <copyright file="ReaddressStatusProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.ReaddressStatus;
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.ReaddressStatus;

    public class ReaddressStatusProfile : Profile
    {
        public ReaddressStatusProfile()
        {
            this.CreateMap<CreateReaddressStatusViewModel, CreateReaddressStatus>();
            this.CreateMap<UpdateReaddressStatusViewModel, UpdateReaddressStatus>();
            this.CreateMap<CreateReaddressStatus, ReaddressStatus>();
            this.CreateMap<UpdateReaddressStatus, ReaddressStatus>();
        }
    }
}
