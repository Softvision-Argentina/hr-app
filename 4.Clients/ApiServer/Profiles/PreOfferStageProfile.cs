using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Services.Contracts.Stage;

namespace ApiServer.Profiles
{
    public class PreOfferStageProfile : Profile
    {
        public PreOfferStageProfile()
        {
            CreateMap<CreatePreOfferStageViewModel, CreatePreOfferStageContract>();
            CreateMap<CreatedPreOfferStageContract, CreatedPreOfferStageViewModel>();
            CreateMap<ReadedPreOfferStageContract, ReadedPreOfferStageViewModel>();
            CreateMap<UpdatePreOfferStageViewModel, UpdatePreOfferStageContract>();
        }
    }
}
