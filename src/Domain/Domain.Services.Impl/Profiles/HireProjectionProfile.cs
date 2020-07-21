using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.HireProjection;

namespace Domain.Services.Impl.Profiles
{
    public class HireProjectionProfile : Profile
    {
        public HireProjectionProfile()
        {
            CreateMap<HireProjection, ReadedHireProjectionContract>();
            CreateMap<CreateHireProjectionContract, HireProjection>();
            CreateMap<HireProjection, CreatedHireProjectionContract>();
            CreateMap<UpdateHireProjectionContract, HireProjection>();
        }
    }
}
