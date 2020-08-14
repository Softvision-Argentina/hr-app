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
            this.CreateMap<ProfileCommunity, ReadedProfileCommunityContract>()
                .ForMember(x => x.CommunityContract, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.ProfileContract, opt => opt.MapFrom(r => r.Profile));
            this.CreateMap<CreateProfileCommunityContract, ProfileCommunity>()
                .ForMember(x => x.Community, opt => opt.Ignore())
                .ForMember(x => x.Profile, opt => opt.Ignore());
            this.CreateMap<ProfileCommunity, CreateProfileCommunityContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
            this.CreateMap<ProfileCommunity, CreatedProfileCommunityContract>();
            this.CreateMap<UpdateProfileCommunityContract, ProfileCommunity>()
                .ForMember(x => x.Community, opt => opt.Ignore())
                .ForMember(x => x.Profile, opt => opt.Ignore());
            this.CreateMap<CreateProfileCommunityViewModel, CreateProfileCommunityContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.Profile));
            this.CreateMap<CreatedProfileCommunityContract, CreatedProfileCommunityViewModel>();
            this.CreateMap<ReadedProfileCommunityContract, ReadedProfileCommunityViewModel>()
                .ForMember(x => x.Profile, opt => opt.MapFrom(r => r.ProfileContract));
            this.CreateMap<UpdateProfileCommunityViewModel, UpdateProfileCommunityContract>()
                .ForMember(x => x.CommunityContract, opt => opt.MapFrom(r => r.Community))
                .ForMember(x => x.ProfileContract, opt => opt.MapFrom(r => r.Profile));
        }
    }
}
