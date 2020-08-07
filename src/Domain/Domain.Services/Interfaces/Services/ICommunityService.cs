// <copyright file="ICommunityService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Community;

    public interface ICommunityService
    {
        CreatedCommunityContract Create(CreateCommunityContract contract);

        ReadedCommunityContract Read(int id);

        void Update(UpdateCommunityContract contract);

        void Delete(int id);

        IEnumerable<ReadedCommunityContract> List();
    }
}
