// <copyright file="OfficeProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.Profiles
{
    using ApiServer.Contracts.Office;
    using AutoMapper;
    using Domain.Services.Contracts.Office;

    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            this.CreateMap<CreateOfficeViewModel, CreateOfficeContract>();
            this.CreateMap<CreatedOfficeContract, CreatedOfficeViewModel>();
            this.CreateMap<ReadedOfficeContract, ReadedOfficeViewModel>();
            this.CreateMap<UpdateOfficeViewModel, UpdateOfficeContract>();
        }
    }
}
