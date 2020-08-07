// <copyright file="ReaddressStatusService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Interfaces.Services;
    using Microsoft.EntityFrameworkCore;

    public class ReaddressStatusService : IReaddressStatusService
    {
        private readonly IMapper mapper;
        private readonly IRepository<ReaddressReason> readdressReasonRepository;
        private readonly IRepository<ReaddressStatus> readdressStatusRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<ReaddressStatusService> log;

        // private readonly UpdateReaddressStatusValidator _updateReaddressStatusValidator;
        // private readonly CreateReaddressStatusValidator _createReaddressStatysValidator;
        public ReaddressStatusService(
            IMapper mapper,
            IRepository<ReaddressReason> readdressReasonRepository,
            IRepository<ReaddressStatus> readdressStatusRepository,
            IUnitOfWork unitOfWork,
            ILog<ReaddressStatusService> log)

        // UpdateReaddressStatusValidator updateReaddressStatusValidator,
        // CreateReaddressStatusValidator createReaddressStatysValidator
        {
            this.mapper = mapper;
            this.readdressReasonRepository = readdressReasonRepository;
            this.readdressStatusRepository = readdressStatusRepository;
            this.unitOfWork = unitOfWork;
            this.log = log;

            // _updateReaddressStatusValidator = updateReaddressStatusValidator;
            // _createReaddressStatysValidator = createReaddressStatysValidator;
        }

        public void Create(int readdressReasonId, ReaddressStatus readdressStatus)
        {
            try
            {
                this.log.LogInformation($"Start {nameof(ReaddressStatusService)}/Create");

                var readdressReason = this.readdressReasonRepository.Get(readdressReasonId);

                if (readdressReason == null)
                {
                    readdressStatus.ReaddressReasonId = null;
                }
                else
                {
                    readdressStatus.ReaddressReason = readdressReason;
                }

                this.readdressStatusRepository.Create(readdressStatus);

                this.log.LogInformation($"End {nameof(ReaddressStatusService)}/Create");
            }
            catch (BusinessValidationException e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressStatusService)}/Create BusinessValidationException: {e.Message}");
                throw e;
            }
            catch (Exception e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressStatusService)}/Create Exception: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public void Update(int readdressReasonId, ReaddressStatus readdressStatus)
        {
            this.log.LogInformation($"Start {nameof(ReaddressStatusService)}/Update");

            try
            {
                var readdressReason = this.readdressReasonRepository.Query().AsNoTracking().FirstOrDefault(_ => _.Id == readdressReasonId);
                if (readdressReason == null)
                {
                    readdressStatus.ReaddressReasonId = null;
                }

                this.readdressStatusRepository.Update(readdressStatus);

                this.log.LogInformation($"End {nameof(ReaddressStatusService)}/Update");
            }
            catch (Exception e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressStatusService)}/Update Exception: {e.Message}");

                throw new BusinessException($"There was an unexpected error: {e.Message}");
            }
        }
    }
}
