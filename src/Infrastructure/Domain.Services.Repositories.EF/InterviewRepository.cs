﻿// <copyright file="InterviewRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class InterviewRepository : Repository<Interview, DataBaseContext>
    {
        public InterviewRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Interview> Query()
        {
            return base.Query();
        }

        public override IQueryable<Interview> QueryEager()
        {
            return this.Query();
        }
    }
}
