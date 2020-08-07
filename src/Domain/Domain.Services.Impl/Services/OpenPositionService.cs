// <copyright file="OpenPositionService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.Community;
    using Domain.Services.Contracts.OpenPositions;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.OpenPosition;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class OpenPositionService : IOpenPositionService
    {
        private readonly IRepository<OpenPosition> openPositionRepository;
        private readonly IRepository<Community> communityRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILog<OpenPosition> log;
        private readonly UpdateOpenPositionContractValidator updateValidator;
        private readonly CreateOpenPositionContractValidator createValidator;

        public OpenPositionService(
            IRepository<OpenPosition> openPositionRepository,
            IRepository<Community> communityRepository,
            IUnitOfWork unitOfWork,
            ILog<OpenPosition> log,
            IMapper mapper,
            UpdateOpenPositionContractValidator updateValidator,
            CreateOpenPositionContractValidator createValidator)
        {
            this.openPositionRepository = openPositionRepository;
            this.communityRepository = communityRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.log = log;
            this.updateValidator = updateValidator;
            this.createValidator = createValidator;
        }

        public CreatedOpenPositionContract Create(CreateOpenPositionContract openPosition)
        {
            this.log.LogInformation($"Validating contract {openPosition.Title}");
            this.ValidateContract(openPosition);

            this.log.LogInformation($"Mapping contract {openPosition.Title}");
            var position = this.mapper.Map<OpenPosition>(openPosition);

            this.AddCommunityToPosition(position, openPosition.Community.Id);

            var createdPosition = this.openPositionRepository.Create(position);
            this.log.LogInformation($"Complete for {openPosition.Title}");

            this.unitOfWork.Complete();

            this.log.LogInformation($"Return {openPosition.Title}");

            return this.mapper.Map<CreatedOpenPositionContract>(createdPosition);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching candidate {id}");
            var position = this.openPositionRepository.Query().FirstOrDefault(p => p.Id == id);

            if (position == null)
            {
                throw new Exception("Position not found");
            }

            this.log.LogInformation($"Deleting position {id}");
            this.openPositionRepository.Delete(position);

            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedOpenPositionContract> Get()
        {
            var positions = this.openPositionRepository.QueryEager().ToList();

            return this.mapper.Map<List<ReadedOpenPositionContract>>(positions);
        }

        public ReadedOpenPositionContract GetById(int id)
        {
            var position = this.openPositionRepository.QueryEager().Where(c => c.Id == id).FirstOrDefault();

            return this.mapper.Map<ReadedOpenPositionContract>(position);
        }

        public void Update(UpdateOpenPositionContract openPosition)
        {
            this.log.LogInformation($"Validating contract {openPosition.Title}");
            this.ValidateContract(openPosition);

            this.log.LogInformation($"Mapping contract {openPosition.Title}");
            var position = this.mapper.Map<OpenPosition>(openPosition);

            this.AddCommunityToPosition(position, openPosition.Community.Id);

            this.openPositionRepository.Update(position);
            this.log.LogInformation($"Complete for {openPosition.Title}");

            this.unitOfWork.Complete();
        }

        private void ValidateContract(CreateOpenPositionContract openPosition)
        {
            try
            {
                this.createValidator.ValidateAndThrow(
                    openPosition,
                    $"{ValidatorConstants.RULESETCREATE}");
            }
            catch (ValidationException ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void ValidateContract(UpdateOpenPositionContract openPosition)
        {
            try
            {
                this.updateValidator.ValidateAndThrow(
                    openPosition,
                    $"{ValidatorConstants.RULESETUPDATE}");
            }
            catch (ValidationException ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void AddCommunityToPosition(OpenPosition position, int communityId)
        {
            var community = this.communityRepository.Query().Where(c => c.Id == communityId).FirstOrDefault();

            if (community == null)
            {
                throw new CommunityNotFoundException(communityId);
            }

            position.Community = community;
        }
    }
}
