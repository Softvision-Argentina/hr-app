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

        public override ClientStage Update(ClientStage clientStage)
        {
            var currentDBInterviews = this.DbContext.Interview.Where(stage => stage.ClientStageId == clientStage.Id);

            this.DbContext.Entry(clientStage).State = EntityState.Modified;
            foreach (Interview interview in clientStage.Interviews)
            {
                if (currentDBInterviews.Any(x => x.Id == interview.Id))
                {
                    this.DbContext.Entry(interview).State = EntityState.Modified;
                }
                else
                {
                    this.DbContext.Entry(interview).State = EntityState.Added;
                    interview.Id = 0;
                }
            }

            this.DbContext.Update(clientStage);

            return clientStage;
        }

        public void UpdateClientStage(ClientStage newStage, ClientStage existingStage)
        {
            this.DbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
