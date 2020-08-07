// <copyright file="CommunityService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.Community;
    using Domain.Services.Contracts.Community;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Community;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class CommunityService : ICommunityService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Community> communityRepository;
        private readonly IRepository<CandidateProfile> candidateProfileRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<CommunityService> log;
        private readonly UpdateCommunityContractValidator updateCommunityContractValidator;
        private readonly CreateCommunityContractValidator createCommunityContractValidator;

        public CommunityService(
            IMapper mapper,
            IRepository<Community> communityRepository,
            IRepository<CandidateProfile> candidateProfileRepository,
            IUnitOfWork unitOfWork,
            ILog<CommunityService> log,
            UpdateCommunityContractValidator updateCommunityContractValidator,
            CreateCommunityContractValidator createCommunityContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.communityRepository = communityRepository;
            this.log = log;
            this.updateCommunityContractValidator = updateCommunityContractValidator;
            this.createCommunityContractValidator = createCommunityContractValidator;
            this.candidateProfileRepository = candidateProfileRepository;
        }

        public CreatedCommunityContract Create(CreateCommunityContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var community = this.mapper.Map<Community>(contract);

            community.Profile = this.candidateProfileRepository.Query().Where(x => x.Id == community.ProfileId).FirstOrDefault();

            var createdCommunity = this.communityRepository.Create(community);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Name}");
            return this.mapper.Map<CreatedCommunityContract>(createdCommunity);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching Community {id}");
            Community community = this.communityRepository.Query().Where(_ => _.Id == id).FirstOrDefault();

            if (community == null)
            {
                throw new DeleteCommunityNotFoundException(id);
            }

            this.log.LogInformation($"Deleting Community {id}");
            this.communityRepository.Delete(community);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateCommunityContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Name}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Name}");
            var community = this.mapper.Map<Community>(contract);

            var updatedCommunity = this.communityRepository.Update(community);
            this.log.LogInformation($"Complete for {contract.Name}");
            this.unitOfWork.Complete();
        }

        public ReadedCommunityContract Read(int id)
        {
            var communityQuery = this.communityRepository
                .Query()
                .Where(_ => _.Id == id)
                .OrderBy(_ => _.Name);

            var communityResult = communityQuery.SingleOrDefault();

            return this.mapper.Map<ReadedCommunityContract>(communityResult);
        }

        public IEnumerable<ReadedCommunityContract> List()
        {
            var communityQuery = this.communityRepository
                .Query()
                .OrderBy(_ => _.Name);

            var communityResult = communityQuery.ToList();

            return this.mapper.Map<List<ReadedCommunityContract>>(communityResult);
        }

        private void ValidateContract(CreateCommunityContract contract)
        {
            try
            {
                this.createCommunityContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateCommunityContract contract)
        {
            try
            {
                this.updateCommunityContractValidator.ValidateAndThrow(
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
