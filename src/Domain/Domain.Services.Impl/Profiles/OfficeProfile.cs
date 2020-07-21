using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Office;

namespace Domain.Services.Impl.Profiles
{
    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            CreateMap<Office, ReadedOfficeContract>();
            CreateMap<CreateOfficeContract, Office>();
            CreateMap<Office, CreatedOfficeContract>();
            CreateMap<UpdateOfficeContract, Office>();
        }
    }
}
