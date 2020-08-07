// <copyright file="IReaddressReasonService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.ReaddressReason;

    public interface IReaddressReasonService
    {
        CreatedReaddressReason Create(CreateReaddressReason contract);

        ReadReaddressReason Read(int id);

        void Update(int id, UpdateReaddressReason contract);

        void Delete(int id);

        IEnumerable<ReadReaddressReason> List();

        IEnumerable<ReadReaddressReason> ListBy(ReaddressReasonSearchModel readdressReasonSearchModel);
    }
}
