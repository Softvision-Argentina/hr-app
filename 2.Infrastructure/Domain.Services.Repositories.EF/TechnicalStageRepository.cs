using Core.Persistance;
using Domain.Model;
using Domain.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Services.Repositories.EF
{
    public class TechnicalStageRepository : Repository<TechnicalStage, DataBaseContext>, ITechnicalStageRepository
    {
        public TechnicalStageRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<TechnicalStage> QueryEager()
        {
            return Query()
                .Include(x => x.UserDelegate)
                .Include(x => x.UserOwner);
        }

        public void UpdateTechnicalStage(TechnicalStage newStage, TechnicalStage existingStage)
        {
            _dbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
