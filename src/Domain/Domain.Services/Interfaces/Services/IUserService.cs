// <copyright file="IUserService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.User;

    public interface IUserService
    {
        ReadedUserContract Authenticate(string username, string password);

        ReadedUserContract AuthenticateExternal(string username);

        IEnumerable<ReadedUserContract> GetAll();

        IEnumerable<ReadedUserContract> GetFilteredForTech();

        IEnumerable<ReadedUserContract> GetFilteredForHr();

        ReadedUserContract GetById(int id);

        ReadedUserRoleContract GetUserRole(string username);
    }
}
