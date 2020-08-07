// <copyright file="CandidateProfileService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.CandidateProfile;
    using Domain.Services.Contracts.CandidateProfile;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.CandidateProfile;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;

    public class CandidateProfileService : ICandidateProfileService
    {
        private readonly IMapper mapper;
        private readonly IRepository<CandidateProfile> candidateProfileRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<CandidateProfileService> log;
        private readonly UpdateCandidateProfileContractValidator updateCandidateProfileContractValidator;
        private readonly CreateCandidateProfileContractValidator createCandidateProfileContractValidator;

        public CandidateProfileService(
            IMapper mapper,
            IRepository<CandidateProfile> candidateProfileRepository,
            IUnitOfWork unitOfWork,
            ILog<CandidateProfileService> log,
            UpdateCandidateProfileContractValidator updateCandidateProfileContractValidator,
            CreateCandidateProfileContractValidator createCandidateProfileContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.candidateProfileRepository = candidateProfileRepository;
            this.log = log;
            this.updateCandidateProfileContractValidator = updateCandidateProfileContractValidator;
            this.createCandidateProfileContractValidator = createCandidateProfileContractValidator;
        }

        public CreatedCandidateProfileContract Create(CreateCandidateProfileContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(0, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var candidateProfile = this.mapper.Map<CandidateProfile>(contract);

            var createdCandidateProfile = this.candidateProfileRepository.Create(candidateProfile);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedCandidateProfileContract>(createdCandidateProfile);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching Candidate Profile {id}");
            var candidateProfile = this.candidateProfileRepository.Query().AsNoTracking().FirstOrDefault(_ => _.Id == id);

            if (candidateProfile == null)
            {
                throw new DeleteCandidateProfileNotFoundException(id);
            }

            this.log.LogInformation($"Deleting Candidate Profile {id}");
            this.candidateProfileRepository.Delete(candidateProfile);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateCandidateProfileContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);
            this.ValidateExistence(contract.Id, contract.Name);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var candidateProfile = this.mapper.Map<CandidateProfile>(contract);

            this.candidateProfileRepository.Update(candidateProfile);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedCandidateProfileContract> List()
        {
            var candidateProfileQuery = this.candidateProfileRepository
                .QueryEager();

            var candidateProfileResult = candidateProfileQuery.ToList();

            return this.mapper.Map<List<ReadedCandidateProfileContract>>(candidateProfileResult);
        }

        public ReadedCandidateProfileContract Read(int id)
        {
            var candidateProfileQuery = this.candidateProfileRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            var candidateProfileResult = candidateProfileQuery.SingleOrDefault();

            return this.mapper.Map<ReadedCandidateProfileContract>(candidateProfileResult);
        }

        private void ValidateContract(CreateCandidateProfileContract contract)
        {
            try
            {
                this.createCandidateProfileContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateCandidateProfileContract contract)
        {
            try
            {
                this.updateCandidateProfileContractValidator.ValidateAndThrow(
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
                var candidateProfile = this.candidateProfileRepository.Query().AsNoTracking().FirstOrDefault(_ => _.Name == name && _.Id != id);
                if (candidateProfile != null)
                {
                    throw new InvalidCandidateProfileException("The Profile already exists .");
                }
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
