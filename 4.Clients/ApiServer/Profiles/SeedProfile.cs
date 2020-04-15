using ApiServer.Contracts.Seed;
using AutoMapper;
using Domain.Services.Contracts.Seed;

namespace ApiServer.Profiles
{
    public class SeedProfile : Profile
    {
        public SeedProfile()
        {
            CreateMap<CreateDummyViewModel, CreateDummyViewModel> ();
            CreateMap<CreatedDummyContract, CreatedDummyViewModel>();
            CreateMap<ReadedDummyContract, ReadedDummyViewModel>();
            CreateMap<UpdateDummyViewModel, UpdateDummyContract>();
        }
    }
}
