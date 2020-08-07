// <copyright file="ReaddressReasonTypeProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.ReaddressReason;

    public class ReaddressReasonTypeProfile : Profile
    {
        public ReaddressReasonTypeProfile()
        {
            this.CreateMap<ReaddressReasonType, ReadReaddressReasonType>().ReverseMap();
            this.CreateMap<ReaddressReasonType, UpdateReaddressReasonType>().ReverseMap();
            this.CreateMap<ReaddressReasonType, CreateReaddressReasonType>().ReverseMap();
            this.CreateMap<ReaddressReasonType, CreatedReaddressReasonType>().ReverseMap();
        }
    }
}
