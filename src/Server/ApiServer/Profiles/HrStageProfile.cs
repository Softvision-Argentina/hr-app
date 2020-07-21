using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Services.Contracts.Stage;

namespace ApiServer.Profiles
{
    public class HrStageProfile : Profile
    {
        public HrStageProfile()
        {
            CreateMap<CreateHrStageViewModel, CreateHrStageContract>();
            CreateMap<CreatedHrStageContract, CreatedHrStageViewModel>();
            CreateMap<ReadedHrStageContract, ReadedHrStageViewModel>();
            CreateMap<UpdateHrStageViewModel, UpdateHrStageContract>();
        }
    }
}
