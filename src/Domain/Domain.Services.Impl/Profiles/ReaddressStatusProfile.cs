// <copyright file="ReaddressStatusProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.ReaddressStatus;

    public class ReaddressStatusProfile : Profile
    {
        public ReaddressStatusProfile()
        {
            this.CreateMap<ReaddressStatus, ReadReaddressStatus>()
                .ForMember(
                    destination => destination.ReaddressReasonId,
                    opt => opt.MapFrom(source => source.ReaddressReason.Id))
                .ForMember(
                    destination => destination.Feedback,
                    opt => opt.MapFrom(source => source.Feedback))
                .ForMember(
                    destination => destination.FromStatus,
                    opt => opt.MapFrom(source => source.FromStatus))
                .ForMember(
                    destination => destination.ToStatus,
                    opt => opt.MapFrom(source => source.ToStatus));
        }
    }
}
