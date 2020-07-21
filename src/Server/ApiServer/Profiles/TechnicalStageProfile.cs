using ApiServer.Contracts.Stage;
using AutoMapper;
using Domain.Services.Contracts.Stage;

namespace ApiServer.Profiles
{
    public class TechnicalStageProfile : Profile
    {
        public TechnicalStageProfile()
        {
            CreateMap<CreateTechnicalStageViewModel, CreateTechnicalStageContract>();
            CreateMap<CreatedTechnicalStageContract, CreatedTechnicalStageViewModel>();
            CreateMap<ReadedTechnicalStageContract, ReadedTechnicalStageViewModel>();
            CreateMap<UpdateTechnicalStageViewModel, UpdateTechnicalStageContract>();
        }
    }
}
