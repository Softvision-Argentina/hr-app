// <copyright file="DeclineReasonService.cs" company="Softvision">
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
    using Domain.Model.Exceptions;
    using Domain.Model.Exceptions.Skill;
    using Domain.Services.Contracts;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class DeclineReasonService : IDeclineReasonService
    {
        private readonly IMapper mapper;
        private readonly IRepository<DeclineReason> declineReasonRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<DeclineReasonService> log;
        private readonly UpdateDeclineReasonContractValidator updateSkillContractValidator;
        private readonly CreateDeclineReasonContractValidator createSkillContractValidator;

        public DeclineReasonService(
            IMapper mapper,
            IRepository<DeclineReason> declineReasonRepository,
            IUnitOfWork unitOfWork,
            ILog<DeclineReasonService> log,
            UpdateDeclineReasonContractValidator updateSkillContractValidator,
            CreateDeclineReasonContractValidator createSkillContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.declineReasonRepository = declineReasonRepository;
            this.log = log;
            this.updateSkillContractValidator = updateSkillContractValidator;
            this.createSkillContractValidator = createSkillContractValidator;
        }

        public CreatedDeclineReasonContract Create(CreateDeclineReasonContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(0, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var declineReason = this.mapper.Map<DeclineReason>(contract);

            var createdDeclineReason = this.declineReasonRepository.Create(declineReason);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedDeclineReasonContract>(createdDeclineReason);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching skill {id}");
            DeclineReason declineReason = this.declineReasonRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (declineReason == null)
            {
                throw new DeleteDeclineReasonNotFoundException(id);
            }

            this.log.LogInformation($"Deleting skill {id}");
            this.declineReasonRepository.Delete(declineReason);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateDeclineReasonContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.Id, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var declineReason = this.mapper.Map<DeclineReason>(contract);

            var updatedSkill = this.declineReasonRepository.Update(declineReason);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedDeclineReasonContract> List()
        {
            var declineReasonQuery = this.declineReasonRepository
                .QueryEager();

            var declineReasonResult = declineReasonQuery.ToList();

            return this.mapper.Map<List<ReadedDeclineReasonContract>>(declineReasonResult);
        }

        public IEnumerable<ReadedDeclineReasonContract> ListNamed()
        {
            var declineReasonQuery = this.declineReasonRepository
                .QueryEager();

            var declineReasonResult = declineReasonQuery.Where(d => !d.Name.Equals("Other"))
                .ToList();

            return this.mapper.Map<List<ReadedDeclineReasonContract>>(declineReasonResult);
        }

        public ReadedDeclineReasonContract Read(int id)
        {
            var declineReasonQuery = this.declineReasonRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var skillResult = declineReasonQuery.SingleOrDefault();

            return this.mapper.Map<ReadedDeclineReasonContract>(skillResult);
        }

        private void ValidateContract(CreateDeclineReasonContract contract)
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

        private void ValidateContract(UpdateDeclineReasonContract contract)
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
                DeclineReason declineReason = this.declineReasonRepository.Query().Where(_ => _.Name == name && _.Id != id).FirstOrDefault();
                if (declineReason != null)
                {
                    throw new InvalidDeclineReasonException("The DeclineReason already exists .");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
