// <copyright file="IDeclineReasonService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts;

    public interface IDeclineReasonService
    {
        CreatedDeclineReasonContract Create(CreateDeclineReasonContract contract);

        ReadedDeclineReasonContract Read(int id);

        void Update(UpdateDeclineReasonContract contract);

        void Delete(int id);

        IEnumerable<ReadedDeclineReasonContract> List();

        IEnumerable<ReadedDeclineReasonContract> ListNamed();
    }
}
