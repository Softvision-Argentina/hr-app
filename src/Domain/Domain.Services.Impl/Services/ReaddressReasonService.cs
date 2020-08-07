// <copyright file="ReaddressReasonService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

    public class ReaddressReasonService : IReaddressReasonService
    {
        private readonly IMapper mapper;
        private readonly IRepository<ReaddressReason> readdressReasonRepository;
        private readonly IRepository<ReaddressReasonType> readdressReasonTypeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<ReaddressReasonService> log;
        private readonly UpdateReaddressReasonContractValidator updateReaddressReasonContractValidator;
        private readonly CreateReaddressReasonContractValidator createReaddressReasonContractValidator;

        public ReaddressReasonService(
            IMapper mapper,
            IRepository<ReaddressReason> readdressReasonRepository,
            IRepository<ReaddressReasonType> readdressReasonTypeRepository,
            IUnitOfWork unitOfWork,
            ILog<ReaddressReasonService> log,
            UpdateReaddressReasonContractValidator updateReaddressReasonContractValidator,
            CreateReaddressReasonContractValidator createReaddressReasonContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.readdressReasonRepository = readdressReasonRepository;
            this.readdressReasonTypeRepository = readdressReasonTypeRepository;
            this.log = log;
            this.updateReaddressReasonContractValidator = updateReaddressReasonContractValidator;
            this.createReaddressReasonContractValidator = createReaddressReasonContractValidator;
        }

        public CreatedReaddressReason Create(CreateReaddressReason contract)
        {
            try
            {
                this.log.LogInformation($"Start {nameof(ReaddressReasonService)}/Create");
                this.ValidateContract(contract);
                if (!this.readdressReasonTypeRepository.Exist(contract.TypeId))
                {
                    throw new ReaddressReasonException($"The reason type with id: {contract.TypeId} does not exist");
                }

                var readdressReason = this.mapper.Map<ReaddressReason>(contract);

                var readdressReasonType = this.readdressReasonTypeRepository.Get(contract.TypeId);
                readdressReason.Type = readdressReasonType;

                this.readdressReasonRepository.Create(readdressReason);

                this.unitOfWork.Complete();
                this.log.LogInformation($"End {nameof(ReaddressReasonService)}/Create");
                return this.mapper.Map<CreatedReaddressReason>(readdressReason);
            }
            catch (BusinessValidationException e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonService)}/Create BusinessValidationException: {e.Message}");
                throw e;
            }
            catch (Exception e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonService)}/Create Exception: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonService)}/Delete{id}");
            try
            {
                var readdressReason = this.readdressReasonRepository.Get(id);
                if (readdressReason != null)
                {
                    this.readdressReasonRepository.Delete(readdressReason);
                    this.unitOfWork.Complete();
                }
                else
                {
                    throw new ReaddressReasonException($"The reason to delete with id: {id} doesn't exists");
                }

                this.log.LogInformation($"End {nameof(ReaddressReasonService)}/{id} Delete");
            }
            catch (BusinessValidationException e)
            {
                this.log.LogError($"Exeption in {nameof(ReaddressReasonService)}/Delete{id} BusinessValidationException: {e.Message}");
                throw e;
            }
            catch (Exception e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonService)}/Delete{id} BusinessValidationException: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public IEnumerable<ReadReaddressReason> List()
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonService)}/List");

            var readdressRasonList = this.readdressReasonRepository
                .QueryEager();

            var declineReasonResult = readdressRasonList.ToList();

            this.log.LogInformation($"End {nameof(ReaddressReasonService)}/List");

            return this.mapper.Map<List<ReadReaddressReason>>(declineReasonResult);
        }

        public IEnumerable<ReadReaddressReason> ListBy(ReaddressReasonSearchModel readdressReasonSearchModel)
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonService)}/ListBy");

            try
            {
                var result = this.readdressReasonRepository.QueryEager();
                var emptyList = new List<ReadReaddressReason>();

                if (readdressReasonSearchModel != null)
                {
                    if (readdressReasonSearchModel.TypeId != default(int))
                    {
                        result = result.Where(_ => _.Type.Id == readdressReasonSearchModel.TypeId);
                    }
                    else
                    {
                        return emptyList;
                    }
                }
                else
                {
                    return emptyList;
                }

                this.log.LogInformation($"End {nameof(ReaddressReasonService)}/ListBy");

                return this.mapper.Map<List<ReadReaddressReason>>(result.ToList());
            }
            catch (BusinessValidationException e)
            {
                this.log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ListBy BusinessValidationException: {e.Message}");

                throw e;
            }
            catch (Exception e)
            {
                this.log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ListBy Exception: {e.Message}");

                throw new BusinessException($"{e.Message}");
            }
        }

        public ReadReaddressReason Read(int id)
        {
            this.log.LogInformation($"{nameof(ReaddressReasonService)}/Read{id}");

            return this.mapper.Map<ReadReaddressReason>(this.readdressReasonRepository.QueryEager().FirstOrDefault(_ => _.Id == id));
        }

        public void Update(int id, UpdateReaddressReason contract)
        {
            this.log.LogInformation($"{nameof(ReaddressReasonService)}/Update{id}");

            try
            {
                this.ValidateContract(contract);
                if (!this.readdressReasonRepository.Exist(id))
                {
                    throw new ReaddressReasonException($"The reason to update with id: {id} does not exist");
                }

                if (!this.readdressReasonTypeRepository.Exist(contract.TypeId))
                {
                    throw new ReaddressReasonException($"The reason type to update with id: {contract.TypeId} does not exist");
                }

                var readdressReasonToUpdate = this.readdressReasonRepository.QueryEager().AsNoTracking().First(_ => _.Id == id);
                var readdressReasonTypeFromContract = this.readdressReasonTypeRepository.Get(contract.TypeId);

                readdressReasonToUpdate.Type = readdressReasonTypeFromContract;
                readdressReasonToUpdate.Name = contract.Name;
                readdressReasonToUpdate.Description = contract.Description;
                readdressReasonToUpdate.Id = id;

                this.readdressReasonRepository.Update(readdressReasonToUpdate);
                this.unitOfWork.Complete();

                this.log.LogInformation($"{nameof(ReaddressReasonService)}/Update{id}");
            }
            catch (BusinessValidationException e)
            {
                this.log.LogError($"Exeption {nameof(ReaddressReasonService)}/Update{id}");

                throw e;
            }
            catch (Exception e)
            {
                this.log.LogError($"Exeption {nameof(ReaddressReasonService)}/Update {id}");

                throw new BusinessException($"{e.Message}");
            }
        }

        private void ValidateContract(CreateReaddressReason contract)
        {
            try
            {
                this.createReaddressReasonContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                this.log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ListBy BusinessValidationException: {ex.Message}");

                throw new CreateInvalidReaddressReasonException(ex.ToListOfMessages());
            }
            catch (Exception e)
            {
                this.log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ListBy BusinessValidationException: {e.Message}");

                throw new BusinessException($"{e.Message}");
            }
        }

        private void ValidateContract(UpdateReaddressReason contract)
        {
            try
            {
                this.log.LogError($"Start {nameof(ReaddressReasonService)}/ValidateContract");

                this.updateReaddressReasonContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETUPDATE}");

                this.log.LogError($"End {nameof(ReaddressReasonService)}/ValidateContract");
            }
            catch (ValidationException ex)
            {
                this.log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ValidateContract ValidationException: {ex.Message}");

                throw new UpdateInvalidReaddressReasonException(ex.ToListOfMessages());
            }
            catch (Exception e)
            {
                this.log.LogError($"Exeption in {nameof(ReaddressReasonService)}/ValidateContract Exception: {e.Message}");

                throw new BusinessException($"{e.Message}");
            }
        }
    }
}
