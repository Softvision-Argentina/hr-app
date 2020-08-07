// <copyright file="IUnitOfWork.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Persistance
{
    public interface IUnitOfWork
    {
        int Complete();
    }
}
