// <copyright file="ReaddressReasonTypeService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.ReaddressReasonType;
    using Domain.Model.Exceptions.Skill;
    using Domain.Services.Contracts.ReaddressReason;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;

    public class ReaddressReasonTypeService : IReaddressReasonTypeService
    {
        private readonly IMapper mapper;
        private readonly IRepository<ReaddressReasonType> readdressReasonTypeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<ReaddressReasonTypeService> log;
        private readonly UpdateReaddressReasonTypeContractValidator updateReaddressReasonTypeContractValidator;
        private readonly CreateReaddressReasonTypeContractValidator createReaddressReasonTypeContractValidator;

        public ReaddressReasonTypeService(
            IMapper mapper,
            IRepository<ReaddressReasonType> readdressReasonTypeRepository,
            IUnitOfWork unitOfWork,
            ILog<ReaddressReasonTypeService> log,
            UpdateReaddressReasonTypeContractValidator updateReaddressReasonTypeContractValidator,
            CreateReaddressReasonTypeContractValidator createReaddressReasonTypeContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.readdressReasonTypeRepository = readdressReasonTypeRepository;
            this.log = log;
            this.updateReaddressReasonTypeContractValidator = updateReaddressReasonTypeContractValidator;
            this.createReaddressReasonTypeContractValidator = createReaddressReasonTypeContractValidator;
        }

        public CreatedReaddressReasonType Create(CreateReaddressReasonType contract)
        {
            try
            {
                this.log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/Create");

                this.ValidateContract(contract);
                this.ValidateDoesNotExists(contract.Name);
                var readdressReasonType = this.readdressReasonTypeRepository.Create(this.mapper.Map<ReaddressReasonType>(contract));
                this.unitOfWork.Complete();

                this.log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/Create");

                return this.mapper.Map<CreatedReaddressReasonType>(readdressReasonType);
            }
            catch (BusinessValidationException e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/Create BusinessValidationException: {e.Message}");
                throw e;
            }
            catch (Exception e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/Create Exception: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/Delete");

            try
            {
                var readdressReason = this.readdressReasonTypeRepository.Get(id);
                if (readdressReason != null)
                {
                    this.readdressReasonTypeRepository.Delete(readdressReason);
                    this.unitOfWork.Complete();

                    this.log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/Delete");
                }
                else
                {
                    this.log.LogError($"Exeption in {nameof(ReaddressReasonTypeService)}/Delete{id} ReaddressReasonTypeException");

                    throw new ReaddressReasonTypeException($"The reason type to delete doesn't exist");
                }
            }
            catch (BusinessValidationException e)
            {
                this.log.LogError($"Exeption in {nameof(ReaddressReasonTypeService)}/Delete{id} BusinessValidationException: {e.Message}");
                throw e;
            }
            catch (Exception e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/Delete{id} BusinessValidationException: {e.Message}");
                throw new BusinessException($"{e.Message}");
            }
        }

        public IEnumerable<ReadReaddressReasonType> List()
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/List");

            var readdressRasonList = this.readdressReasonTypeRepository
                .Query();

            this.log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/List");

            return this.mapper.Map<List<ReadReaddressReasonType>>(readdressRasonList.ToList());
        }

        public ReadReaddressReasonType Read(int id)
        {
            this.log.LogInformation($"{nameof(ReaddressReasonTypeService)}/Read{id}");

            return this.mapper.Map<ReadReaddressReasonType>(this.readdressReasonTypeRepository.Query().FirstOrDefault(_ => _.Id == id));
        }

        public void Update(int id, UpdateReaddressReasonType contract)
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/Update/{id}");

            try
            {
                this.ValidateContract(contract);

                if (!this.readdressReasonTypeRepository.Exist(id))
                {
                    throw new ReaddressReasonTypeException($"The reason to update with id: {id} does not exist");
                }

                var readdressReasonTypeToUpdate = this.readdressReasonTypeRepository.Query().AsNoTracking().First(_ => _.Id == id);
                var readdressReasonTypeFromContract = this.mapper.Map<ReaddressReasonType>(contract);

                readdressReasonTypeToUpdate = readdressReasonTypeFromContract;
                readdressReasonTypeToUpdate.Id = id;

                this.readdressReasonTypeRepository.Update(readdressReasonTypeToUpdate);
                this.unitOfWork.Complete();

                this.log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/Update/{id}");
            }
            catch (Exception e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/Update{id} Exception: {e.Message}");

                throw new BusinessException($"There was an unexpected error: {e.Message}");
            }
        }

        private void ValidateContract(CreateReaddressReasonType contract)
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/ValidateContract");

            try
            {
                this.createReaddressReasonTypeContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/ValidateContract ValidationException: {ex.Message}");

                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }

            this.log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/ValidateContract");
        }

        private void ValidateContract(UpdateReaddressReasonType contract)
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/ValidateContract");

            try
            {
                this.updateReaddressReasonTypeContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETUPDATE}");
            }
            catch (ValidationException ex)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/ValidateContract CreateContractInvalidException: {ex.Message}");

                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }

            this.log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/ValidateContract");
        }

        private void ValidateDoesNotExists(string name)
        {
            this.log.LogInformation($"Start {nameof(ReaddressReasonTypeService)}/ValidateDoesNotExists");

            try
            {
                var exists = this.readdressReasonTypeRepository.Query().AsNoTracking().FirstOrDefault(_ => _.Name == name) != null;
                if (exists)
                {
                    throw new ReaddressReasonTypeException($"There is already a reason with name {name}");
                }
            }
            catch (ReaddressReasonTypeException ex)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/ValidateDoesNotExists ReaddressReasonTypeException: {ex.Message}");

                throw ex;
            }
            catch (Exception e)
            {
                this.log.LogError($"Exception in {nameof(ReaddressReasonTypeService)}/ValidateDoesNotExists Exception: {e.Message}");

                throw new BusinessException($"There was an unexpected error: {e.Message}");
            }

            this.log.LogInformation($"End {nameof(ReaddressReasonTypeService)}/ValidateDoesNotExists");
        }
    }
}
