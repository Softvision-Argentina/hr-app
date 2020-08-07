// <copyright file="IInterviewService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Interview;

    public interface IInterviewService
    {
        CreatedInterviewContract Create(CreateInterviewContract contract);

        ReadedInterviewContract Read(int id);

        void Update(UpdateInterviewContract contract);

        void Delete(int id);

        IEnumerable<ReadedInterviewContract> List();
    }
}