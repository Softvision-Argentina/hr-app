// <copyright file="IReaddressStatusService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using Domain.Model;

    public interface IReaddressStatusService
    {
        void Create(int readdressReasonId, ReaddressStatus contract);

        void Update(int readdressReasonId, ReaddressStatus contract);
    }
}
