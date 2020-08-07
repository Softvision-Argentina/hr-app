// <copyright file="PostulantRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Persistance.EF;

    public class PostulantRepository : Repository<Postulant, DataBaseContext>
    {
        public PostulantRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Postulant> Query()
        {
            return base.Query();
        }

        public override IQueryable<Postulant> QueryEager()
        {
            return this.Query();
        }
    }
}
