// <copyright file="HireProjectionService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Model.Exceptions.HireProjection;
    using Domain.Services.Contracts.HireProjection;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.HireProjection;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class HireProjectionService : IHireProjectionService
    {
        private readonly IMapper mapper;
        private readonly IRepository<HireProjection> hireProjectionRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<HireProjectionService> log;
        private readonly UpdateHireProjectionContractValidator updateHireProjectionContractValidator;
        private readonly CreateHireProjectionContractValidator createHireProjectionContractValidator;

        public HireProjectionService(
            IMapper mapper,
            IRepository<HireProjection> hireProjectionRepository,
            IUnitOfWork unitOfWork,
            ILog<HireProjectionService> log,
            UpdateHireProjectionContractValidator updateHireProjectionContractValidator,
            CreateHireProjectionContractValidator createHireProjectionContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.hireProjectionRepository = hireProjectionRepository;
            this.log = log;
            this.updateHireProjectionContractValidator = updateHireProjectionContractValidator;
            this.createHireProjectionContractValidator = createHireProjectionContractValidator;
        }

        public IEnumerable<ReadedHireProjectionContract> List()
        {
            var hireProjectionQuery = this.hireProjectionRepository.QueryEager();

            var hireProjections = hireProjectionQuery.ToList();

            return this.mapper.Map<List<ReadedHireProjectionContract>>(hireProjections);
        }

        public CreatedHireProjectionContract Create(CreateHireProjectionContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Month} + {contract.Year}");
            this.ValidateContract(contract);
            this.ValidateExistence(0, contract.Month, contract.Year);

            this.log.LogInformation($"Mapping contract {contract.Month} + {contract.Year}");
            var hireProjection = this.mapper.Map<HireProjection>(contract);

            var createdHireProjection = this.hireProjectionRepository.Create(hireProjection);
            this.log.LogInformation($"Complete for {contract.Month} + {contract.Year}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Month} + {contract.Year}");
            return this.mapper.Map<CreatedHireProjectionContract>(createdHireProjection);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching projection {id}");
            HireProjection hireProjection = this.hireProjectionRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (hireProjection == null)
            {
                throw new DeleteHireProjectionNotFoundException(id);
            }

            this.log.LogInformation($"Deleting hireProjection {id}");
            this.hireProjectionRepository.Delete(hireProjection);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateHireProjectionContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Month} + {contract.Year}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.Id, contract.Month, contract.Year);

            this.log.LogInformation($"Mapping contract {contract.Month} + {contract.Year}");
            var hireProjection = this.mapper.Map<HireProjection>(contract);

            var updatedHireProjection = this.hireProjectionRepository.Update(hireProjection);
            this.log.LogInformation($"Complete for {contract.Month} + {contract.Year}");
            this.unitOfWork.Complete();
        }

        public ReadedHireProjectionContract Read(int id)
        {
            var hireProjectionQuery = this.hireProjectionRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var hireProjectionResult = hireProjectionQuery.SingleOrDefault();

            return this.mapper.Map<ReadedHireProjectionContract>(hireProjectionResult);
        }

        private void ValidateContract(CreateHireProjectionContract contract)
        {
            try
            {
                this.createHireProjectionContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateHireProjectionContract contract)
        {
            try
            {
                this.updateHireProjectionContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateExistence(int id, int month, int year)
        {
            try
            {
                HireProjection hireProjection = this.hireProjectionRepository.Query().Where(_ => _.Month == month && _.Year == year && _.Id != id).FirstOrDefault();
                if (hireProjection != null)
                {
                    throw new InvalidHireProjectionException("The HireProjection already exists .");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
