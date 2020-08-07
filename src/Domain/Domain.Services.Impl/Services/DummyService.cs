// <copyright file="DummyService.cs" company="Softvision">
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
    using Domain.Model.Seed;
    using Domain.Model.Seed.Exceptions;
    using Domain.Services.Contracts.Seed;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Seed;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class DummyService : IDummyService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Dummy> dummyRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<DummyService> log;
        private readonly UpdateDummyContractValidator updateDummyContractValidator;
        private readonly CreateDummyContractValidator createDummyContractValidator;

        public DummyService(
            IMapper mapper,
            IRepository<Dummy> dummyRepository,
            IUnitOfWork unitOfWork,
            ILog<DummyService> log,
            UpdateDummyContractValidator updateDummyContractValidator,
            CreateDummyContractValidator createDummyContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.dummyRepository = dummyRepository;
            this.log = log;
            this.updateDummyContractValidator = updateDummyContractValidator;
            this.createDummyContractValidator = createDummyContractValidator;
        }

        public CreatedDummyContract Create(CreateDummyContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var dummy = this.mapper.Map<Dummy>(contract);

            var createdDummy = this.dummyRepository.Create(dummy);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedDummyContract>(createdDummy);
        }

        public void Delete(Guid id)
        {
            this.log.LogInformation($"Searching dummy {id}");
            Dummy dummy = this.dummyRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (dummy == null)
            {
                throw new DeleteDummyNotFoundException(id);
            }

            this.log.LogInformation($"Deleting dummy {id}");
            this.dummyRepository.Delete(dummy);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateDummyContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var dummy = this.mapper.Map<Dummy>(contract);

            var updatedDummy = this.dummyRepository.Update(dummy);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedDummyContract> List()
        {
            var dummyQuery = this.dummyRepository
                .QueryEager();

            var dummyResult = dummyQuery.ToList();

            return this.mapper.Map<List<ReadedDummyContract>>(dummyResult);
        }

        public ReadedDummyContract Read(Guid id)
        {
            var dummyQuery = this.dummyRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var dummyResult = dummyQuery.SingleOrDefault();

            return this.mapper.Map<ReadedDummyContract>(dummyResult);
        }

        private void ValidateContract(CreateDummyContract contract)
        {
            try
            {
                this.createDummyContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateDummyContract contract)
        {
            try
            {
                this.updateDummyContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
