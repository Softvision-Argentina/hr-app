using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.PreOffer;

namespace Domain.Services.Impl.Profiles
{
    public class PreOfferProfile : Profile
    {
        public PreOfferProfile()
        {
            CreateMap<PreOffer, ReadedPreOfferContract>();
            CreateMap<ReadedPreOfferContract, PreOffer>();
            CreateMap<CreatePreOfferContract, PreOffer>();
            CreateMap<DeclineReason, CreatedPreOfferContract>();
            CreateMap<UpdatePreOfferContract, PreOffer>();
        }
    }
}
