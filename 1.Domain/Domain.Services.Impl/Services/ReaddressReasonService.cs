using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.ReaddressReason;
using Domain.Services.Contracts.ReaddressReason;
using Domain.Services.Impl.Validators;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class ReaddressReasonService : IReaddressReasonService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ReaddressReason> _readdressReasonRepository;
        private readonly IRepository<ReaddressReasonType> _readdressReasonTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<ReaddressReasonService> _log;
        private readonly UpdateReaddressReasonContractValidator _updateReaddressReasonContractValidator;
        private readonly CreateReaddressReasonContractValidator _createReaddressReasonContractValidator;

        public ReaddressReasonService(
            IMapper mapper,
            IRepository<ReaddressReason> readdressReasonRepository,
            IRepository<ReaddressReasonType> readdressReasonTypeRepository,
            IUnitOfWork unitOfWork,
            ILog<ReaddressReasonService> log,
            UpdateReaddressReasonContractValidator updateReaddressReasonContractValidator,
            CreateReaddressReasonContractValidator createReaddressReasonContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _readdressReasonRepository = readdressReasonRepository;
            _readdressReasonTypeRepository = readdressReasonTypeRepository;
            _log = log;
            _updateReaddressReasonContractValidator = updateReaddressReasonContractValidator;
            _createReaddressReasonContractValidator = createReaddressReasonContractValidator;
        }

        public CreatedReaddressReason Create(CreateReaddressReason contract)
        {
            try
            {
                _log.LogInformation($"Start {nameof(ReaddressReasonService)}/Create");
                ValidateContract(contract);
                if (!_readdressReasonTypeRepository.Exist(contract.TypeId))
                    throw new ReaddressReasonException($"The reason type with id: {contract.TypeId} does not exist");

                var readdressReason = _mapper.Map<ReaddressReason>(contract);

                var readdressReasonType = _readdressReasonTypeRepository.Get(contract.TypeId);
                readdressReason.Type = readdressReasonType;
                
                _readdressReasonRepository.Create(readdressReason);

                _unitOfWork.Complete();
                _log.LogInformation($"End {nameof(ReaddressReasonService)}/Create");
                return _mapper.Map<CreatedReaddressReason>(readdressReason);
            }
            catch (BusinessValidationException e)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonService)}/Create BusinessValidationException: {e.Message}");
                throw e;
            }
            catch (Exception e)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonService)}/Create Exception: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonService)}/Delete{id}");
            try
            {
                var readdressReason = _readdressReasonRepository.Get(id);
                if (readdressReason != null)
                {
                    _readdressReasonRepository.Delete(readdressReason);
                    _unitOfWork.Complete();
                }
                else
                    throw new ReaddressReasonException($"The reason to delete with id: {id} doesn't exists");
                _log.LogInformation($"End {nameof(ReaddressReasonService)}/{id} Delete");
            }

            catch (BusinessValidationException e)
            {
                _log.LogError($"Exeption in {nameof(ReaddressReasonService)}/Delete{id} BusinessValidationException: {e.Message}");
                throw e;
            }

            catch (Exception e)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonService)}/Delete{id} BusinessValidationException: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public IEnumerable<ReadReaddressReason> List()
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonService)}/List");

            var readdressRasonList = _readdressReasonRepository
                .QueryEager();

            var declineReasonResult = readdressRasonList.ToList();

            _log.LogInformation($"End {nameof(ReaddressReasonService)}/List");

            return _mapper.Map<List<ReadReaddressReason>>(declineReasonResult);
        }

        public IEnumerable<ReadReaddressReason> ListBy(ReaddressReasonSearchModel readdressReasonSearchModel)
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonService)}/ListBy");

            try
            {
                var result = _readdressReasonRepository.QueryEager();
                var emptyList = new List<ReadReaddressReason>();

                if (readdressReasonSearchModel != null)
                {
                    if (readdressReasonSearchModel.TypeId != default(int))
                    {
                        result = result.Where(_ => _.Type.Id == readdressReasonSearchModel.TypeId);
                    }
                    else
                        return emptyList;
                }
                else
                    return emptyList;

                _log.LogInformation($"End {nameof(ReaddressReasonService)}/ListBy");

                return _mapper.Map<List<ReadReaddressReason>>(result.ToList());
            }

            catch (BusinessValidationException e)
            {
                _log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ListBy BusinessValidationException: {e.Message}");

                throw e;
            }

            catch (Exception e)
            {
                _log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ListBy Exception: {e.Message}");

                throw new BusinessException($"{e.Message}");
            }
        }

        public ReadReaddressReason Read(int id)
        {
            _log.LogInformation($"{nameof(ReaddressReasonService)}/Read{id}");

            return _mapper.Map<ReadReaddressReason>(_readdressReasonRepository.QueryEager().FirstOrDefault(_ => _.Id == id));
        }

        public void Update(int id, UpdateReaddressReason contract)
        {
            _log.LogInformation($"{nameof(ReaddressReasonService)}/Update{id}");

            try
            {
                ValidateContract(contract);
                if (!_readdressReasonRepository.Exist(id))
                    throw new ReaddressReasonException($"The reason to update with id: {id} does not exist");

                if (!_readdressReasonTypeRepository.Exist(contract.TypeId))
                    throw new ReaddressReasonException($"The reason type to update with id: {contract.TypeId} does not exist");

                var readdressReasonToUpdate = _readdressReasonRepository.QueryEager().AsNoTracking().First(_ => _.Id == id);
                var readdressReasonTypeFromContract = _readdressReasonTypeRepository.Get(contract.TypeId);

                readdressReasonToUpdate.Type = readdressReasonTypeFromContract;
                readdressReasonToUpdate.Name = contract.Name;
                readdressReasonToUpdate.Description = contract.Description;
                readdressReasonToUpdate.Id = id;

                _readdressReasonRepository.Update(readdressReasonToUpdate);
                _unitOfWork.Complete();
                
                _log.LogInformation($"{nameof(ReaddressReasonService)}/Update{id}");
            }
            catch (BusinessValidationException e)
            {
                _log.LogError($"Exeption {nameof(ReaddressReasonService)}/Update{id}");

                throw e;
            }

            catch (Exception e)
            {
                _log.LogError($"Exeption {nameof(ReaddressReasonService)}/Update {id}");

                throw new BusinessException($"{e.Message}");
            }
        }

        private void ValidateContract(CreateReaddressReason contract)
        {
            try
            {
                _createReaddressReasonContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                _log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ListBy BusinessValidationException: {ex.Message}");

                throw new CreateInvalidReaddressReasonException(ex.ToListOfMessages());
            }

            catch (Exception e)
            {
                _log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ListBy BusinessValidationException: {e.Message}");

                throw new BusinessException($"{e.Message}");
            }
        }

        private void ValidateContract(UpdateReaddressReason contract)
        {
            try
            {
                _log.LogError($"Start {nameof(ReaddressReasonService)}/ValidateContract");

                _updateReaddressReasonContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_UPDATE}");

                _log.LogError($"End {nameof(ReaddressReasonService)}/ValidateContract");
            }
            catch (ValidationException ex)
            {
                _log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ValidateContract ValidationException: {ex.Message}");

                throw new UpdateInvalidReaddressReasonException(ex.ToListOfMessages());
            }

            catch (Exception e)
            {
                _log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ValidateContract Exception: {e.Message}");

                throw new BusinessException($"{e.Message}");
            }
        }
    }
}
