// <copyright file="ClientStageRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class ClientStageRepository : Repository<ClientStage, DataBaseContext>, IClientStageRepository
    {
        public ClientStageRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<ClientStage> QueryEager()
        {
            return this.Query()
                .Include(x => x.UserDelegate)
                .Include(x => x.UserOwner)
                .Include(x => x.Interviews);
        }

        public void UpdateClientStage(ClientStage newStage, ClientStage existingStage)
        {
            this.DbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
