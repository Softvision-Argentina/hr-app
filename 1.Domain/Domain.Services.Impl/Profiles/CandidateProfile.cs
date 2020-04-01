using AutoMapper;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.Candidate;
using System;

namespace Domain.Services.Impl.Profiles
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<Candidate, ReadedCandidateContract>()
                .ForMember(x => x.User, opt => opt.MapFrom(r => r.User))
                .ForMember(x => x.PreferredOfficeId, opt => opt.MapFrom(r => r.PreferredOffice.Id))
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));

            CreateMap<CreateCandidateContract, Candidate>()
                .ForMember(destination => destination.EnglishLevel,
                opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)))
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.PreferredOffice, opt => opt.Ignore())
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));

            CreateMap<Candidate, CreateCandidateContract>()
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));

            CreateMap<Candidate, CreatedCandidateContract>();

            CreateMap<Candidate, ReadedCandidateAppContract>()
                .ForMember(x => x.User, opt => opt.MapFrom(r => r.User))
                .ForMember(x => x.PreferredOffice, opt => opt.MapFrom(r => r.PreferredOffice))
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));

            CreateMap<UpdateCandidateContract, Candidate>()
                .ForMember(destination => destination.EnglishLevel,
                opt => opt.MapFrom(source => Enum.GetName(typeof(EnglishLevel), source.EnglishLevel)))
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.PreferredOffice, opt => opt.Ignore())
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
        }
    }
}
