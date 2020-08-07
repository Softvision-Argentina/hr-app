// <copyright file="DeclineReasonProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts;

    public class DeclineReasonProfile : Profile
    {
        public DeclineReasonProfile()
        {
            this.CreateMap<DeclineReason, ReadedDeclineReasonContract>();
            this.CreateMap<ReadedDeclineReasonContract, DeclineReason>();
            this.CreateMap<CreateDeclineReasonContract, DeclineReason>();
            this.CreateMap<DeclineReason, CreatedDeclineReasonContract>();
            this.CreateMap<UpdateDeclineReasonContract, DeclineReason>();
        }
    }
}
