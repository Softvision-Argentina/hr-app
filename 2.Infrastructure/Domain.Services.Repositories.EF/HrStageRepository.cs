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
    public class HrStageRepository : Repository<HrStage, DataBaseContext>, IHrStageRepository
    {
        public HrStageRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<HrStage> QueryEager()
        {
            return Query()
                .Include(x => x.UserDelegate)
                .Include(x => x.UserOwner);
        }

        public void UpdateHrStage(HrStage newStage, HrStage existingStage)
        {
            _dbContext.Entry(existingStage).CurrentValues.SetValues(newStage);
        }
    }
}
