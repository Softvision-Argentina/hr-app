using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;

namespace Domain.Services.Repositories.EF
{
    public class PreferenceRepository : Repository<Preference, DataBaseContext>
    {
        public PreferenceRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Preference> Query()
        {
            return base.Query();
        }

        public override IQueryable<Preference> QueryEager()
        {
            return Query().Include(c => c.User);
        }

        /*public override Preference Update(Preference entity)
        {
            var previousItem = _dbContext.Preferences.Where(t => t.Id == entity.Id);
            _dbContext.Preferences.RemoveRange(previousItem);
           
            return base.Update(entity);
        }
        */
    }
}
