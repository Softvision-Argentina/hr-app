// <copyright file="IRoleService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Role;

    public interface IRoleService
    {
        CreatedRoleContract Create(CreateRoleContract contract);

        ReadedRoleContract Read(int id);

        void Update(UpdateRoleContract contract);

        void Delete(int id);

        IEnumerable<ReadedRoleContract> List();
    }
}
