using ApiServer.Contracts.PreOffer;
using AutoMapper;
using Domain.Services.Contracts.PreOffer;

namespace ApiServer.Profiles
{
    public class PreOfferProfile : Profile
    {
        public PreOfferProfile()
        {
            CreateMap<CreatePreOfferViewModel, CreatePreOfferContract>();
            CreateMap<CreatedPreOfferContract, CreatedPreOfferViewModel>();
            CreateMap<ReadedPreOfferContract, ReadedPreOfferViewModel>();
            CreateMap<UpdatePreOfferViewModel, UpdatePreOfferContract>();
        }
    }
}
