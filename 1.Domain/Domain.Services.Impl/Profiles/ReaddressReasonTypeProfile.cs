using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Process;
using Domain.Services.Contracts.ReaddressReason;

namespace Domain.Services.Impl.Profiles
{
    public class ReaddressReasonTypeProfile : Profile
    {
        public ReaddressReasonTypeProfile()
        {
            CreateMap<ReaddressReasonType, ReadReaddressReasonType>().ReverseMap();
            CreateMap<ReaddressReasonType, UpdateReaddressReasonType>().ReverseMap();
            CreateMap<ReaddressReasonType, CreateReaddressReasonType>().ReverseMap();
            CreateMap<ReaddressReasonType, CreatedReaddressReasonType>().ReverseMap();
        }
    }
}
