// <copyright file="UnitOfWork.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Persistance.EF
{
    using System;
    using Core.Persistance;
    using Core.Persistance.Exceptions;
    using Microsoft.EntityFrameworkCore;

    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        protected TContext DbContext { get; private set; }

        public UnitOfWork(TContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public int Complete()
        {
            try
            {
                return this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new CannotCommitTransactionException("An error occurred while committing unit of work changes.", ex);
            }
        }
    }
}
