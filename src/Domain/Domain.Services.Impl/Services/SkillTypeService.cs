// <copyright file="SkillTypeService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.Skill;
    using Domain.Model.Exceptions.SkillType;
    using Domain.Services.Contracts.SkillType;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.SkillType;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class SkillTypeService : ISkillTypeService
    {
        private readonly IMapper mapper;
        private readonly IRepository<SkillType> skillTypeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<SkillTypeService> log;
        private readonly UpdateSkillTypeContractValidator updateSkillContractValidator;
        private readonly CreateSkillTypeContractValidator createSkillContractValidator;

        public SkillTypeService(
            IMapper mapper,
            IRepository<SkillType> skillTypeRepository,
            IUnitOfWork unitOfWork,
            ILog<SkillTypeService> log,
            UpdateSkillTypeContractValidator updateSkillContractValidator,
            CreateSkillTypeContractValidator createSkillContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.skillTypeRepository = skillTypeRepository;
            this.log = log;
            this.updateSkillContractValidator = updateSkillContractValidator;
            this.createSkillContractValidator = createSkillContractValidator;
        }

        public CreatedSkillTypeContract Create(CreateSkillTypeContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(0, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var skillType = this.mapper.Map<SkillType>(contract);

            var createdSkillType = this.skillTypeRepository.Create(skillType);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedSkillTypeContract>(createdSkillType);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching skill {id}");
            SkillType skillType = this.skillTypeRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (skillType == null)
            {
                throw new DeleteSkillNotFoundException(id);
            }

            this.log.LogInformation($"Deleting skill {id}");
            this.skillTypeRepository.Delete(skillType);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateSkillTypeContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.Id, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var skillType = this.mapper.Map<SkillType>(contract);

            this.skillTypeRepository.Update(skillType);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedSkillTypeContract> List()
        {
            var skillTypeQuery = this.skillTypeRepository
                .QueryEager();

            var skillTypeResult = skillTypeQuery.ToList();

            return this.mapper.Map<List<ReadedSkillTypeContract>>(skillTypeResult);
        }

        public ReadedSkillTypeContract Read(int id)
        {
            var skillTypeQuery = this.skillTypeRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var skillResult = skillTypeQuery.SingleOrDefault();

            return this.mapper.Map<ReadedSkillTypeContract>(skillResult);
        }

        private void ValidateContract(CreateSkillTypeContract contract)
        {
            try
            {
                this.createSkillContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateSkillTypeContract contract)
        {
            try
            {
                this.updateSkillContractValidator.ValidateAndThrow(
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
                SkillType skillType = this.skillTypeRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (skillType != null)
                {
                    throw new InvalidSkillTypeException("The SkillType already exists .");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
