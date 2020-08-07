// <copyright file="EmployeeCasualtyService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.EmployeeCasualty;
    using Domain.Services.Contracts.EmployeeCasualty;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.EmployeeCasualty;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class EmployeeCasualtyService : IEmployeeCasualtyService
    {
        private readonly IMapper mapper;
        private readonly IRepository<EmployeeCasualty> employeeCasualtyRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<EmployeeCasualtyService> log;
        private readonly UpdateEmployeeCasualtyContractValidator updateEmployeeCasualtyContractValidator;
        private readonly CreateEmployeeCasualtyContractValidator createEmployeeCasualtyContractValidator;

        public EmployeeCasualtyService(
            IMapper mapper,
            IRepository<EmployeeCasualty> employeeCasualtyRepository,
            IUnitOfWork unitOfWork,
            ILog<EmployeeCasualtyService> log,
            UpdateEmployeeCasualtyContractValidator updateEmployeeCasualtyContractValidator,
            CreateEmployeeCasualtyContractValidator createEmployeeCasualtyContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.employeeCasualtyRepository = employeeCasualtyRepository;
            this.log = log;
            this.updateEmployeeCasualtyContractValidator = updateEmployeeCasualtyContractValidator;
            this.createEmployeeCasualtyContractValidator = createEmployeeCasualtyContractValidator;
        }

        public IEnumerable<ReadedEmployeeCasualtyContract> List()
        {
            var employeeCasualtyQuery = this.employeeCasualtyRepository.QueryEager();

            var employeeCasualties = employeeCasualtyQuery.ToList();

            return this.mapper.Map<List<ReadedEmployeeCasualtyContract>>(employeeCasualties);
        }

        public CreatedEmployeeCasualtyContract Create(CreateEmployeeCasualtyContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Month} + {contract.Year}");
            this.ValidateContract(contract);
            this.ValidateExistence(0, contract.Month, contract.Year);

            this.log.LogInformation($"Mapping contract {contract.Month} + {contract.Year}");
            var employeeCasualty = this.mapper.Map<EmployeeCasualty>(contract);

            var createdEmployeeCasualty = this.employeeCasualtyRepository.Create(employeeCasualty);
            this.log.LogInformation($"Complete for {contract.Month} + {contract.Year}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Month} + {contract.Year}");
            return this.mapper.Map<CreatedEmployeeCasualtyContract>(createdEmployeeCasualty);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching casualty {id}");
            EmployeeCasualty employeeCasualty = this.employeeCasualtyRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (employeeCasualty == null)
            {
                throw new DeleteEmployeeCasualtyNotFoundException(id);
            }

            this.log.LogInformation($"Deleting employeeCasualty {id}");
            this.employeeCasualtyRepository.Delete(employeeCasualty);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateEmployeeCasualtyContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Month} + {contract.Year}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.Id, contract.Month, contract.Year);

            this.log.LogInformation($"Mapping contract {contract.Month} + {contract.Year}");
            var employeeCasualty = this.mapper.Map<EmployeeCasualty>(contract);

            var updatedEmployeeCasualty = this.employeeCasualtyRepository.Update(employeeCasualty);
            this.log.LogInformation($"Complete for {contract.Month} + {contract.Year}");
            this.unitOfWork.Complete();
        }

        public ReadedEmployeeCasualtyContract Read(int id)
        {
            var employeeCasualtyQuery = this.employeeCasualtyRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var employeeCasualtyResult = employeeCasualtyQuery.SingleOrDefault();

            return this.mapper.Map<ReadedEmployeeCasualtyContract>(employeeCasualtyResult);
        }

        private void ValidateContract(CreateEmployeeCasualtyContract contract)
        {
            try
            {
                this.createEmployeeCasualtyContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateEmployeeCasualtyContract contract)
        {
            try
            {
                this.updateEmployeeCasualtyContractValidator.ValidateAndThrow(
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
                EmployeeCasualty employeeCasualty = this.employeeCasualtyRepository.Query().Where(_ => _.Month == month && _.Year == year && _.Id != id).FirstOrDefault();
                if (employeeCasualty != null)
                {
                    throw new InvalidEmployeeCasualtyException("The EmployeeCasualty already exists .");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
