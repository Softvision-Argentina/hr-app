using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.ReaddressReason;
using Domain.Services.Contracts.ReaddressStatus;

namespace Domain.Services.Impl.Profiles
{
    public class ReaddressStatusProfile : Profile
    {
        public ReaddressStatusProfile()
        {
            CreateMap<ReaddressStatus, ReadReaddressStatus>()
                .ForMember(destination => destination.ReaddressReasonId,
                    opt => opt.MapFrom(source => source.ReaddressReason.Id))
                .ForMember(destination => destination.Feedback,
                    opt => opt.MapFrom(source => source.Feedback))
                .ForMember(destination => destination.FromStatus,
                    opt => opt.MapFrom(source => source.FromStatus))
                .ForMember(destination => destination.ToStatus,
                    opt => opt.MapFrom(source => source.ToStatus));
        }
    }
}
