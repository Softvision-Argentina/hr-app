// <copyright file="CvProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Cv;

    public class CvProfile : Profile
    {
        public CvProfile()
        {
            this.CreateMap<CvContractAdd, Cv>();
        }
    }
}
