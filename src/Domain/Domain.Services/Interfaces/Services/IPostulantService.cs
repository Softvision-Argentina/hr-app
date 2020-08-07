// <copyright file="IPostulantService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Postulant;

    public interface IPostulantService
    {
        ReadedPostulantContract Read(int id);

        IEnumerable<ReadedPostulantContract> List();
    }
}
