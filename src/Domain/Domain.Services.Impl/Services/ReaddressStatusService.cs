using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.ReaddressStatus;
using Domain.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class ReaddressStatusService : IReaddressStatusService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ReaddressReason> _readdressReasonRepository;
        private readonly IRepository<ReaddressStatus> _readdressStatusRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<ReaddressStatusService> _log;
        //private readonly UpdateReaddressStatusValidator _updateReaddressStatusValidator;
        //private readonly CreateReaddressStatusValidator _createReaddressStatysValidator;

        public ReaddressStatusService(
            IMapper mapper,
            IRepository<ReaddressReason> readdressReasonRepository,
            IRepository<ReaddressStatus> readdressStatusRepository,
            IUnitOfWork unitOfWork,
            ILog<ReaddressStatusService> log
            //UpdateReaddressStatusValidator updateReaddressStatusValidator,
            //CreateReaddressStatusValidator createReaddressStatysValidator
            )
        {
            _mapper = mapper;
            _readdressReasonRepository = readdressReasonRepository;
            _readdressStatusRepository = readdressStatusRepository;
            _unitOfWork = unitOfWork;
            _log = log;
            //_updateReaddressStatusValidator = updateReaddressStatusValidator;
            //_createReaddressStatysValidator = createReaddressStatysValidator;
        }

        public void Create(int readdressReasonId, ReaddressStatus readdressStatus)
        {
            try
            {
                _log.LogInformation($"Start {nameof(ReaddressStatusService)}/Create");

                var readdressReason = _readdressReasonRepository.Get(readdressReasonId);
                
                if (readdressReason == null)
                    readdressStatus.ReaddressReasonId = null;
                else
                    readdressStatus.ReaddressReason = readdressReason;

                _readdressStatusRepository.Create(readdressStatus);

                _log.LogInformation($"End {nameof(ReaddressStatusService)}/Create");
            }

            catch (BusinessValidationException e)
            {
                _log.LogError($"Exception in {nameof(ReaddressStatusService)}/Create BusinessValidationException: {e.Message}");
                throw e;
            }
            catch (Exception e)
            {
                _log.LogError($"Exception in {nameof(ReaddressStatusService)}/Create Exception: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public void Update(int readdressReasonId, ReaddressStatus readdressStatus)
        {
            _log.LogInformation($"Start {nameof(ReaddressStatusService)}/Update");

            try
            {
                var readdressReason = _readdressReasonRepository.Query().AsNoTracking().FirstOrDefault(_ => _.Id == readdressReasonId);
                if (readdressReason == null)
                    readdressStatus.ReaddressReasonId = null;

                _readdressStatusRepository.Update(readdressStatus);

                _log.LogInformation($"End {nameof(ReaddressStatusService)}/Update");
            }

            catch (Exception e)
            {
                _log.LogError($"Exception in {nameof(ReaddressStatusService)}/Update Exception: {e.Message}");

                throw new BusinessException($"There was an unexpected error: {e.Message}");
            }
        }
    }
}
