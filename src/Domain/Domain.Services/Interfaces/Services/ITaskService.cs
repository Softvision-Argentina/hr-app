// <copyright file="ITaskService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Services
{
    using System.Collections.Generic;
    using Domain.Services.Contracts.Task;

    public interface ITaskService
    {
        CreatedTaskContract Create(CreateTaskContract contract);

        ReadedTaskContract Read(int id);

        IEnumerable<ReadedTaskContract> ListByUser(string userEmail);

        void Update(UpdateTaskContract contract);

        void Approve(int id);

        void Delete(int id);

        IEnumerable<ReadedTaskContract> List();
    }
}
