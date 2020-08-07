// <copyright file="ReaddressReasonProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.ReaddressReason;

    public class ReaddressReasonProfile : Profile
    {
        public ReaddressReasonProfile()
        {
            this.CreateMap<ReaddressReason, UpdateReaddressReason>().ReverseMap();

            this.CreateMap<ReaddressReason, CreateReaddressReason>()
                .ForMember(
                    destination => destination.TypeId,
                    opt => opt.MapFrom(source => source.Type.Id))
                .ReverseMap();

            this.CreateMap<ReaddressReason, ReadReaddressReason>()
                .ForMember(
                    destination => destination.Type,
                    opt => opt.MapFrom(source => source.Type.Name))
                .ReverseMap();

            this.CreateMap<ReaddressReason, CreatedReaddressReason>().ReverseMap();
        }
    }
}
