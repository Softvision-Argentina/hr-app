// <copyright file="IReaddressReasonTypeService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.ReaddressReason;

    public interface IReaddressReasonTypeService
    {
        CreatedReaddressReasonType Create(CreateReaddressReasonType contract);

        ReadReaddressReasonType Read(int id);

        void Update(int id, UpdateReaddressReasonType contract);

        void Delete(int id);

        IEnumerable<ReadReaddressReasonType> List();
    }
}
