// <copyright file="OfficeProfile.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Profiles
{
    using AutoMapper;
    using Domain.Model;
    using Domain.Services.Contracts.Office;

    public class OfficeProfile : Profile
    {
        public OfficeProfile()
        {
            this.CreateMap<Office, ReadedOfficeContract>();
            this.CreateMap<CreateOfficeContract, Office>();
            this.CreateMap<Office, CreatedOfficeContract>();
            this.CreateMap<UpdateOfficeContract, Office>();
        }
    }
}
