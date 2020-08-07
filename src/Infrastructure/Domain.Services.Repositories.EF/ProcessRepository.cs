// <copyright file="ProcessRepository.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Repositories.EF
{
    using System;
    using System.Linq;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Model.Enum;
    using Domain.Services.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Persistance.EF;

    public class ProcessRepository : Repository<Process, DataBaseContext>, IProcessRepository
    {
        public ProcessRepository(DataBaseContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        public override IQueryable<Process> QueryEager()
        {
            return this.Query()
                .Include(x => x.HrStage)
                    .ThenInclude(x => x.ReaddressStatus)
                        .ThenInclude(x => x.ReaddressReason)
                .Include(x => x.TechnicalStage)
                    .ThenInclude(x => x.ReaddressStatus)
                        .ThenInclude(x => x.ReaddressReason)
                .Include(x => x.ClientStage)
                    .ThenInclude(x => x.ReaddressStatus)
                        .ThenInclude(x => x.ReaddressReason)
                .Include(x => x.ClientStage)
                    .ThenInclude(x => x.Interviews)
                .Include(x => x.PreOfferStage)
                    .ThenInclude(x => x.ReaddressStatus)
                        .ThenInclude(x => x.ReaddressReason)
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
            var entity = this.QueryEager().Where(p => p.Id == id).FirstOrDefault();
            if (entity != null)
            {
                entity.RejectionReason = null;
                entity.Status = Model.Enum.ProcessStatus.InProgress;
                entity.EndDate = DateTime.UtcNow;
                entity.Candidate.Status = Model.Enum.CandidateStatus.InProgress;
                entity.HrStage.Status = StageStatus.Accepted;
                entity.TechnicalStage.Status = StageStatus.Accepted;
                entity.ClientStage.Status = StageStatus.Accepted;
                entity.PreOfferStage.Status = StageStatus.Accepted;
                entity.OfferStage.Status = StageStatus.InProgress;
            }
        }

        /// <summary>
        /// Candidato pasa a estado rejected, y los stages asociados al proceso que estan sin terminar
        /// pasan a estado cancelado.
        /// </summary>
        /// <param name="id">ID del proceso.</param>
        public void Reject(int id, string rejectionReason)
        {
            var entity = this.QueryEager().Where(p => p.Id == id).FirstOrDefault();
            if (entity != null)
            {
                entity.Status = Model.Enum.ProcessStatus.Rejected;
                entity.Candidate.Status = Model.Enum.CandidateStatus.Rejected;
                entity.RejectionReason = rejectionReason;

                entity.HrStage.Status = this.RejectStage(entity.HrStage.Status);
                entity.TechnicalStage.Status = this.RejectStage(entity.TechnicalStage.Status);
                entity.ClientStage.Status = this.RejectStage(entity.ClientStage.Status);
                entity.PreOfferStage.Status = this.RejectStage(entity.PreOfferStage.Status);
                entity.OfferStage.Status = this.RejectStage(entity.OfferStage.Status);
            }
        }

        public Process GetByIdFullProcess(int id)
        {
            return this.Query().Where(x => x.Id == id)
                .Include(x => x.HrStage)
                .Include(x => x.TechnicalStage)
                .Include(x => x.ClientStage)
                .Include(x => x.PreOfferStage)
                .Include(x => x.OfferStage)
                .Include(x => x.UserOwner)
                .Include(x => x.UserDelegate).FirstOrDefault();
        }

        public StageStatus RejectStage(StageStatus currentStatus)
        {
            return currentStatus != StageStatus.Accepted ? StageStatus.Rejected : currentStatus;
        }

        public override bool Exist(int id)
        {
            return this.Query().AsNoTracking().FirstOrDefault(_ => _.Id == id) != null;
        }
    }
}
