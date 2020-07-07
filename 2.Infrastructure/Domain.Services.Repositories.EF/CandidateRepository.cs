using Core.Persistance;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class CandidateRepository : Repository<Candidate, DataBaseContext>
    {
        public CandidateRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {

        }

        public override IQueryable<Candidate> Query()
        {
            return base.Query();
        }

        public override IQueryable<Candidate> QueryEager()
        {
            return Query().Include(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
                .Include(r => r.User)
                .Include(r => r.PreferredOffice)
                .Include(r => r.Community)
                .Include(r => r.Profile)
                .Include(r => r.OpenPosition);

        }

        public override Candidate Update(Candidate entity)
        {
            //Remuevo previo set de skills del candidato. El usuario puede haber creado, eliminado o editado existentes
            var previousSkills = _dbContext.CandidateSkills.Where(cs => cs.CandidateId == entity.Id);
            _dbContext.CandidateSkills.RemoveRange(previousSkills);

            foreach (var item in entity.CandidateSkills)
            {
                _dbContext.CandidateSkills.Add(item);
            }            

            return base.Update(entity);
        }

        public List<Candidate> GetReferralsList(string user)
        {
            return _dbContext.Candidates.Where(w => w.ReferredBy == user).ToList();
        }
    }
}
