using AutoMapper;
using Domain.Model;
using Domain.Services.Contracts.Cv;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services.Impl.Profiles
{
    public class CvProfile : Profile
    {
        public CvProfile()
        {
            CreateMap<Cv, CvContractReturn>();
            CreateMap<CvContractAdd, Cv>();
        }
    }
}
