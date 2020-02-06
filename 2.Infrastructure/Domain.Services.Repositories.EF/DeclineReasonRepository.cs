using Core.Persistance;
using Domain.Model;
using Persistance.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Services.Repositories.EF
{
    public class DeclineReasonRepository : Repository<DeclineReason, DataBaseContext>
    {
        public DeclineReasonRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<DeclineReason> Query()
        {
            return base.Query();
        }

        public override IQueryable<DeclineReason> QueryEager()
        {
            return Query();
        }
    }
}
