// <copyright file="OfficeService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.Office;
    using Domain.Services.Contracts.Office;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Office;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class OfficeService : IOfficeService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Office> officeRepository;
        private readonly IRepository<Model.Room> roomItemRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<OfficeService> log;
        private readonly UpdateOfficeContractValidator updateOfficeContractValidator;
        private readonly CreateOfficeContractValidator createOfficeContractValidator;

        public OfficeService(
            IMapper mapper,
            IRepository<Office> officeRepository,
            IRepository<Model.Room> roomItemRepository,
            IUnitOfWork unitOfWork,
            ILog<OfficeService> log,
            UpdateOfficeContractValidator updateOfficeContractValidator,
            CreateOfficeContractValidator createOfficeContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.officeRepository = officeRepository;
            this.roomItemRepository = roomItemRepository;
            this.log = log;
            this.updateOfficeContractValidator = updateOfficeContractValidator;
            this.createOfficeContractValidator = createOfficeContractValidator;
        }

        public CreatedOfficeContract Create(CreateOfficeContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(0, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var office = this.mapper.Map<Office>(contract);

            var createdOffice = this.officeRepository.Create(office);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedOfficeContract>(createdOffice);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching Candidate Profile {id}");
            Office office = this.officeRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (office == null)
            {
                throw new DeleteOfficeNotFoundException(id);
            }

            this.log.LogInformation($"Deleting Candidate Profile {id}");
            this.officeRepository.Delete(office);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateOfficeContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.Id, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var office = this.mapper.Map<Office>(contract);

            var updatedOffice = this.officeRepository.Update(office);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedOfficeContract> List()
        {
            var officeQuery = this.officeRepository
                .QueryEager();

            var officeResult = officeQuery.ToList();

            return this.mapper.Map<List<ReadedOfficeContract>>(officeResult);
        }

        public ReadedOfficeContract Read(int id)
        {
            var officeQuery = this.officeRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var officeResult = officeQuery.SingleOrDefault();

            return this.mapper.Map<ReadedOfficeContract>(officeResult);
        }

        private void ValidateContract(CreateOfficeContract contract)
        {
            try
            {
                this.createOfficeContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateOfficeContract contract)
        {
            try
            {
                this.updateOfficeContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateExistence(int id, string name)
        {
            try
            {
                Office office = this.officeRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (office != null)
                {
                    throw new InvalidOfficeException("The Office already exists .");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
