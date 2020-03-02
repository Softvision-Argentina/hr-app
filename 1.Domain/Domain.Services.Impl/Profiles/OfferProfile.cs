using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Offer;

namespace Domain.Services.Impl.Profiles
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            CreateMap<Offer, ReadedOfferContract>();
            CreateMap<ReadedOfferContract, Offer>();
            CreateMap<CreateOfferContract, Offer>();
            CreateMap<DeclineReason, CreatedOfferContract>();
            CreateMap<UpdateOfferContract, Offer>();
        }
    }
}
