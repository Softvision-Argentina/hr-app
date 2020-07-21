using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.ReaddressReasonType;
using Domain.Model.Exceptions.Skill;
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
    public class ReaddressReasonTypeService : IReaddressReasonTypeService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ReaddressReasonType> _readdressReasonTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<ReaddressReasonTypeService> _log;
        private readonly UpdateReaddressReasonTypeContractValidator _updateReaddressReasonTypeContractValidator;
        private readonly CreateReaddressReasonTypeContractValidator _createReaddressReasonTypeContractValidator;

        public ReaddressReasonTypeService(
            IMapper mapper,
            IRepository<ReaddressReasonType> readdressReasonTypeRepository,
            IUnitOfWork unitOfWork,
            ILog<ReaddressReasonTypeService> log,
            UpdateReaddressReasonTypeContractValidator updateReaddressReasonTypeContractValidator,
            CreateReaddressReasonTypeContractValidator createReaddressReasonTypeContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _readdressReasonTypeRepository = readdressReasonTypeRepository;
            _log = log;
            _updateReaddressReasonTypeContractValidator = updateReaddressReasonTypeContractValidator;
            _createReaddressReasonTypeContractValidator = createReaddressReasonTypeContractValidator;
        }

        public CreatedReaddressReasonType Create(CreateReaddressReasonType contract)
        {
            try
            {
                _log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/Create");

                ValidateContract(contract);
                ValidateDoesNotExists(contract.Name);
                var readdressReasonType = _readdressReasonTypeRepository.Create(_mapper.Map<ReaddressReasonType>(contract));
                _unitOfWork.Complete();

                _log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/Create");

                return _mapper.Map<CreatedReaddressReasonType>(readdressReasonType);
            }

            catch (BusinessValidationException e)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/Create BusinessValidationException: {e.Message}");
                throw e;
            }
            catch (Exception e)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/Create Exception: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/Delete");

            try
            {
                var readdressReason = _readdressReasonTypeRepository.Get(id);
                if (readdressReason != null)
                {
                    _readdressReasonTypeRepository.Delete(readdressReason);
                    _unitOfWork.Complete();

                    _log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/Delete");
                }
                else
                {
                    _log.LogError($"Exeption in {nameof(ReaddressReasonTypeService)}/Delete{id} ReaddressReasonTypeException");

                    throw new ReaddressReasonTypeException($"The reason type to delete doesn't exist");
                }
            }
            catch (BusinessValidationException e)
            {
                _log.LogError($"Exeption in {nameof(ReaddressReasonTypeService)}/Delete{id} BusinessValidationException: {e.Message}");
                throw e;
            }

            catch (Exception e)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/Delete{id} BusinessValidationException: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public IEnumerable<ReadReaddressReasonType> List()
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/List");

            var readdressRasonList = _readdressReasonTypeRepository
                .Query();

            _log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/List");

            return _mapper.Map<List<ReadReaddressReasonType>>(readdressRasonList.ToList());
        }

        public ReadReaddressReasonType Read(int id)
        {
            _log.LogInformation($"{nameof(ReaddressReasonTypeService)}/Read{id}");

            return _mapper.Map<ReadReaddressReasonType>(_readdressReasonTypeRepository.Query().FirstOrDefault(_ => _.Id == id));
        }

        public void Update(int id, UpdateReaddressReasonType contract)
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/Update/{id}");

            try
            {
                ValidateContract(contract);

                if (!_readdressReasonTypeRepository.Exist(id))
                    throw new ReaddressReasonTypeException($"The reason to update with id: {id} does not exist");

                var readdressReasonTypeToUpdate = _readdressReasonTypeRepository.Query().AsNoTracking().First(_ => _.Id == id);
                var readdressReasonTypeFromContract = _mapper.Map<ReaddressReasonType>(contract);

                readdressReasonTypeToUpdate = readdressReasonTypeFromContract;
                readdressReasonTypeToUpdate.Id = id;

                _readdressReasonTypeRepository.Update(readdressReasonTypeToUpdate);
                _unitOfWork.Complete();

                _log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/Update/{id}");
            }

            catch (Exception e)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/Update{id} Exception: {e.Message}");

                throw new BusinessException($"There was an unexpected error: {e.Message}");
            }
        }

        private void ValidateContract(CreateReaddressReasonType contract)
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/ValidateContract");

            try
            {
                _createReaddressReasonTypeContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/ValidateContract ValidationException: {ex.Message}");

                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }

            _log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/ValidateContract");
        }

        private void ValidateContract(UpdateReaddressReasonType contract)
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/ValidateContract");

            try
            {
                _updateReaddressReasonTypeContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_UPDATE}");
            }
            catch (ValidationException ex)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/ValidateContract CreateContractInvalidException: {ex.Message}");

                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }

            _log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/ValidateContract");
        }

        private void ValidateDoesNotExists(string name)
        {
            _log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/ValidateDoesNotExists");

            try
            {
                var exists = _readdressReasonTypeRepository.Query().AsNoTracking().FirstOrDefault(_ => _.Name == name) != null;
                if (exists) throw new ReaddressReasonTypeException($"There is already a reason with name {name}");
            }
            catch (ReaddressReasonTypeException ex)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/ValidateDoesNotExists ReaddressReasonTypeException: {ex.Message}");

                throw ex;
            }
            catch (Exception e)
            {
                _log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/ValidateDoesNotExists Exception: {e.Message}");

                throw new BusinessException($"There was an unexpected error: {e.Message}");
            }

            _log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/ValidateDoesNotExists");
        }
    }
}
