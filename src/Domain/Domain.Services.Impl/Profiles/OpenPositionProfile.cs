using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.OpenPositions;

namespace Domain.Services.Impl.Profiles
{
    public class OpenPositionProfile : Profile
    {
        public OpenPositionProfile()
        {
            CreateMap<OpenPosition, ReadedOpenPositionContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));

            CreateMap<CreateOpenPositionContract, OpenPosition>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));

            CreateMap<OpenPosition, CreateOpenPositionContract>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));

            CreateMap<OpenPosition, CreatedOpenPositionContract>();

            CreateMap<UpdateOpenPositionContract, OpenPosition>()
                .ForMember(x => x.Community, opt => opt.MapFrom(r => r.Community));
        }
    }
}
