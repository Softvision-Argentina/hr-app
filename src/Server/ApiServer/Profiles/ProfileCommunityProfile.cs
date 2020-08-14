namespace Domain.Services.Impl.Profiles
{
    using ApiServer.Contracts.ProfileByCommunity;
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.ProfileByCommunity;

    public class ProfileCommunityProfile : Profile
    {
        public ProfileCommunityProfile()
        {
            CreateMap<ProfileCommunity, ReadedProfileCommunityContract>()
                .ForMember(x => x.CommunityContract, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.ProfileContract, opt => opt.MapFrom(r => r.Profile));
            CreateMap<CreateProfileCommunityContract, ProfileCommunity>()
                .ForMember(x => x.Community, opt => opt.Ignore())
                .ForMember(x => x.Profile, opt => opt.Ignore());
            CreateMap<ProfileCommunity, CreateProfileCommunityContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
            CreateMap<ProfileCommunity, CreatedProfileCommunityContract>();
            CreateMap<UpdateProfileCommunityContract, ProfileCommunity>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.CommunityContract))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.ProfileContract));
            CreateMap<CreateProfileCommunityViewModel, CreateProfileCommunityContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
            CreateMap<CreatedProfileCommunityContract, CreatedProfileCommunityViewModel>();
            CreateMap<ReadedProfileCommunityContract, ReadedProfileCommunityViewModel>()
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.ProfileContract));
            CreateMap<UpdateProfileCommunityViewModel, UpdateProfileCommunityContract>()
                .ForMember(x => x.CommunityContract, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.ProfileContract, opt => opt.MapFrom(r => r.Profile));
        }
    }
}
