using Core.Persistance;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistance.EF;
using System;
using System.Linq;

namespace Domain.Services.Repositories.EF
{
    public class ProcessRepository : Repository<Process, DataBaseContext>, IProcessRepository
    {
        public ProcessRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Process> QueryEager()
        {
            return Query()         
                .Include(x => x.HrStage)
                .Include(x => x.TechnicalStage)
                .Include(x => x.ClientStage)
                .Include(x => x.OfferStage)
                .Include(x => x.UserOwner)
                .Include(x => x.UserDelegate)
                .Include(x => x.DeclineReason)
                .Include(x => x.Candidate)
                .ThenInclude(c => c.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .Include(x => x.Candidate.User)
                .Include(x => x.Candidate.PreferredOffice)
                .Include(x => x.Candidate.Community)
                .Include(x => x.Candidate.Profile);
        }

        public void Approve(int id)
        {
            var entity = QueryEager().Where(p => p.Id == id).FirstOrDefault();
            if (entity != null)
            {
                entity.RejectionReason = null;
                entity.Status = Model.Enum.ProcessStatus.InProgress;
                entity.EndDate = DateTime.UtcNow;
                entity.Candidate.Status = Model.Enum.CandidateStatus.InProgress;
                entity.HrStage.Status = StageStatus.Accepted;
                entity.TechnicalStage.Status = StageStatus.Accepted;
                entity.ClientStage.Status = StageStatus.Accepted;
                entity.OfferStage.Status = StageStatus.InProgress;
            }
        }

        /// <summary>
        /// Candidato pasa a estado rejected, y los stages asociados al proceso que estan sin terminar
        /// pasan a estado cancelado
        /// </summary>
        /// <param name="id">ID del proceso</param>
        public void Reject(int id, string rejectionReason)
        {
            var entity = QueryEager().Where(p => p.Id == id).FirstOrDefault();
            if (entity != null)
            {
                entity.Status = Model.Enum.ProcessStatus.Rejected;
                entity.Candidate.Status = Model.Enum.CandidateStatus.Rejected;
                entity.RejectionReason = rejectionReason;

                entity.HrStage.Status = RejectStage(entity.HrStage.Status);
                entity.TechnicalStage.Status = RejectStage(entity.TechnicalStage.Status);
                entity.ClientStage.Status = RejectStage(entity.ClientStage.Status);
                entity.OfferStage.Status = RejectStage(entity.OfferStage.Status);
            }
        }

        public Process GetByIdFullProcess(int id)
        {
            return Query().Where(x => x.Id == id)
                .Include(x => x.HrStage)
                .Include(x => x.TechnicalStage)
                .Include(x => x.ClientStage)
                .Include(x => x.OfferStage)
                .Include(x => x.UserOwner)
                .Include(x => x.UserDelegate).FirstOrDefault();
        }

        public StageStatus RejectStage(StageStatus currentStatus)
        {
            return currentStatus != StageStatus.Accepted ? StageStatus.Rejected : currentStatus;
        }
    }
}
