using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Cv;

namespace Domain.Services.Impl.Profiles
{
    public class CvProfile : Profile
    {
        public CvProfile()
        {
            CreateMap<CvContractAdd, Cv>();
        }
    }
}
