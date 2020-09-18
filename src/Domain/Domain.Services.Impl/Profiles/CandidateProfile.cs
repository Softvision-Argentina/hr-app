// <copyright file="CandidateProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using System;
    using AutoMapper;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Contracts.Candidate;

    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            this.CreateMap<Candidate, ReadedCandidateContract>()
                .ForMember(x => x.User, opt => opt.MapFrom(r => r.User))
                .ForMember(x => x.PreferredOfficeId, opt => opt.MapFrom(r => r.PreferredOffice.Id))
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile))
                .ForMember(x => x.OpenPosition, opt => opt.Ignore());

            this.CreateMap<CreateCandidateContract, Candidate>()
                .ForMember(
                    destination => destination.EnglishLevel,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)))
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.PreferredOffice, opt => opt.Ignore())
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile))
                .ForMember(x => x.OpenPosition, opt => opt.Ignore());

            this.CreateMap<Candidate, CreateCandidateContract>()
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile))
                .ForMember(x => x.OpenPosition, opt => opt.Ignore());

            this.CreateMap<Candidate, CreatedCandidateContract>();

            this.CreateMap<Candidate, ReadedCandidateAppContract>()
                .ForMember(x => x.User, opt => opt.MapFrom(r => r.User))
                .ForMember(x => x.PreferredOffice, opt => opt.MapFrom(r => r.PreferredOffice))
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile))
                .ForMember(x => x.OpenPosition, opt => opt.Ignore());

            this.CreateMap<UpdateCandidateContract, Candidate>()
                .ForMember(
                    destination => destination.EnglishLevel,
                    opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)))
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.PreferredOffice, opt => opt.Ignore())
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile))
                .ForMember(x => x.OpenPosition, opt => opt.Ignore());
        }
    }
}
