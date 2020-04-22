using ApiServer.Contracts.Offer;
using AutoMapper;
using Domain.Services.Contracts.Offer;

namespace ApiServer.Profiles
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            CreateMap<CreateOfferViewModel, CreateOfferContract>();
            CreateMap<CreatedOfferContract, CreatedOfferViewModel>();
            CreateMap<ReadedOfferContract, ReadedOfferViewModel>();
            CreateMap<UpdateOfferViewModel, UpdateOfferContract>();
        }
    }
}
