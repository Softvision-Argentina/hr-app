using ApiServer.Contracts.Office;
using Domain.Services.Contracts.Office;
using AutoMapper;

namespace ApiServer.Profiles
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            CreateMap<CreateOfficeViewModel, CreateOfficeContract>();
            CreateMap<CreatedOfficeContract, CreatedOfficeViewModel>();
            CreateMap<ReadedOfficeContract, ReadedOfficeViewModel>();
            CreateMap<UpdateOfficeViewModel, UpdateOfficeContract>();
        }
    }
}
