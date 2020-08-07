// <copyright file="IRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Persistance.EF
{
    using Core;
    using Core.Persistance;
    using Microsoft.EntityFrameworkCore;

    public interface IRepository<TEntity, TContext> : IRepository, IRepository<TEntity> where TEntity : class, IEntity where TContext : DbContext
    {
    }
}
