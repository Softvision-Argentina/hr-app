// <copyright file="IOfficeService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Office;

    public interface IOfficeService
    {
        CreatedOfficeContract Create(CreateOfficeContract contract);

        ReadedOfficeContract Read(int id);

        void Update(UpdateOfficeContract contract);

        void Delete(int id);

        IEnumerable<ReadedOfficeContract> List();
    }
}
