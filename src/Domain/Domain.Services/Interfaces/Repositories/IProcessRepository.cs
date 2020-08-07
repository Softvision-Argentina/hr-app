// <copyright file="IProcessRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Interfaces.Repositories
{
    using Core.Persistance;
    using Domain.Model;

    public interface IProcessRepository : IRepository<Process>
    {
        Process GetByIdFullProcess(int id);

        void Approve(int id);

        void Reject(int id, string rejectionReason);
    }
}
