using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Services.Contracts.Stage;

namespace ApiServer.Profiles
{
    public class StageProfile : Profile
    {
        public StageProfile()
        {
            CreateMap<CreateStageViewModel, CreateStageContract>();
            CreateMap<CreatedStageContract, CreatedStageViewModel>();
            CreateMap<ReadedStageContract, ReadedStageViewModel>();
            CreateMap<UpdateStageViewModel, UpdateStageContract>();
        }
    }
}
