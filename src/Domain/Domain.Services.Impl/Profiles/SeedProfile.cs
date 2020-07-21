using AutoMapper;
using Domain.Model.Seed;
using Domain.Services.Contracts.Seed;

namespace Domain.Services.Impl.Profiles
{
    public class SeedProfile : Profile
    {
        public SeedProfile()
        {
            CreateMap<Dummy, ReadedDummyContract>();
            CreateMap<CreateDummyContract, Dummy>();
            CreateMap<Dummy, CreatedDummyContract>();
            CreateMap<UpdateDummyContract, Dummy>();
        }
    }
}
