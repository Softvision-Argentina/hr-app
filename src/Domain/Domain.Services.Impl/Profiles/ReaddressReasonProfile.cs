using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.ReaddressReason;

namespace Domain.Services.Impl.Profiles
{
    public class ReaddressReasonProfile : Profile
    {
        public ReaddressReasonProfile()
        {

            CreateMap<ReaddressReason, UpdateReaddressReason>().ReverseMap();

            CreateMap<ReaddressReason, CreateReaddressReason>()
                .ForMember(destination => destination.TypeId,
                    opt => opt.MapFrom(source => source.Type.Id))
                .ReverseMap();

            CreateMap<ReaddressReason, ReadReaddressReason>()
                .ForMember(destination => destination.Type,
                    opt => opt.MapFrom(source => source.Type.Name))
                .ReverseMap();

            CreateMap<ReaddressReason, CreatedReaddressReason>().ReverseMap();

        }
    }
}
