using ApiServer.Contracts.HireProjection;
using AutoMapper;
using Domain.Services.Contracts.HireProjection;

namespace ApiServer.Profiles
{
    public class HireProjectionProfile : Profile
    {
        public HireProjectionProfile()
        {
            CreateMap<CreateHireProjectionViewModel, CreateHireProjectionContract>();
            CreateMap<CreatedHireProjectionContract, CreatedHireProjectionViewModel>();
            CreateMap<ReadedHireProjectionContract, ReadedHireProjectionViewModel>();
            CreateMap<UpdateHireProjectionViewModel, UpdateHireProjectionContract>();
        }
    }
}
