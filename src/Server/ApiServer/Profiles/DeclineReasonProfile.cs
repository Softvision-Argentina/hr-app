// <copyright file="DeclineReasonProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts;
    using AutoMapper;
    using Domain.Services.Contracts;

    public class DeclineReasonProfile : Profile
    {
        public DeclineReasonProfile()
        {
            this.CreateMap<CreateDeclineReasonViewModel, CreateDeclineReasonContract>();
            this.CreateMap<CreatedDeclineReasonContract, CreatedDeclineReasonViewModel>();
            this.CreateMap<ReadedDeclineReasonContract, ReadedDeclineReasonViewModel>();
            this.CreateMap<UpdateDeclineReasonViewModel, UpdateDeclineReasonContract>();
        }
    }
}
