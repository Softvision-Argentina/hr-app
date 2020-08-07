// <copyright file="SkillService.cs" company="Softvision">
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
    using Domain.Services.Contracts.Skill;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Skill;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class SkillService : ISkillService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Skill> skillRepository;
        private readonly IRepository<SkillType> skillTypesRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<SkillService> log;
        private readonly UpdateSkillContractValidator updateSkillContractValidator;
        private readonly CreateSkillContractValidator createSkillContractValidator;

        public SkillService(
            IMapper mapper,
            IRepository<Skill> skillRepository,
            IRepository<SkillType> skillTypesRepository,
            IUnitOfWork unitOfWork,
            ILog<SkillService> log,
            UpdateSkillContractValidator updateSkillContractValidator,
            CreateSkillContractValidator createSkillContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.skillRepository = skillRepository;
            this.skillTypesRepository = skillTypesRepository;
            this.log = log;
            this.updateSkillContractValidator = updateSkillContractValidator;
            this.createSkillContractValidator = createSkillContractValidator;
        }

        public CreatedSkillContract Create(CreateSkillContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(0, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var skill = this.mapper.Map<Skill>(contract);

            this.AddTypeToSkill(skill, contract.Type);

            var createdSkill = this.skillRepository.Create(skill);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedSkillContract>(createdSkill);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching skill {id}");
            Skill skill = this.skillRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (skill == null)
            {
                throw new DeleteSkillNotFoundException(id);
            }

            this.log.LogInformation($"Deleting skill {id}");
            this.skillRepository.Delete(skill);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateSkillContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.Id, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var skill = this.mapper.Map<Skill>(contract);

            this.AddTypeToSkill(skill, contract.Type);

            this.skillRepository.Update(skill);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        private void AddTypeToSkill(Skill skill, int typeId)
        {
            var type = this.skillTypesRepository.Query().Where(_ => _.Id == typeId).FirstOrDefault();
            if (type == null)
            {
                throw new SkillTypeNotFoundException(typeId);
            }

            skill.Type = type;
        }

        public IEnumerable<ReadedSkillContract> List()
        {
            var skillQuery = this.skillRepository
                .QueryEager();

            var skillResult = skillQuery.ToList();

            return this.mapper.Map<List<ReadedSkillContract>>(skillResult);
        }

        public ReadedSkillContract Read(int id)
        {
            var skillQuery = this.skillRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var skillResult = skillQuery.SingleOrDefault();

            return this.mapper.Map<ReadedSkillContract>(skillResult);
        }

        private void ValidateContract(CreateSkillContract contract)
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

        private void ValidateContract(UpdateSkillContract contract)
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
                Skill skill = this.skillRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (skill != null)
                {
                    throw new InvalidSkillException("The skill already exists .");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
