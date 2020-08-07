// <copyright file="ProcessStageService.cs" company="Softvision">
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
    using Domain.Model.Exceptions.Stage;
    using Domain.Services.Contracts.Process;
    using Domain.Services.Contracts.Stage;
    using Domain.Services.Contracts.Stage.StageItem;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Stage;
    using Domain.Services.Interfaces.Repositories;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class ProcessStageService : IProcessStageService
    {
        private readonly IMapper mapper;
        private readonly IProcessStageRepository processStageRepository;
        private readonly IStageItemRepository stageItemRepository;
        private readonly IProcessRepository processRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<ProcessStageService> log;
        private readonly UpdateStageContractValidator updateStageContractValidator;
        private readonly CreateStageContractValidator createStageContractValidator;
        private readonly ProcessStatusContractValidator processStatusContractValidator;

        public ProcessStageService(
            IMapper mapper,
            IProcessStageRepository processStageRepository,
            IStageItemRepository stageItemRepository,
            IProcessRepository processRepository,
            IUnitOfWork unitOfWork,
            ILog<ProcessStageService> log,
            UpdateStageContractValidator updateStageContractValidator,
            CreateStageContractValidator createStageContractValidator,
            ProcessStatusContractValidator processStatusContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.processStageRepository = processStageRepository;
            this.stageItemRepository = stageItemRepository;
            this.processRepository = processRepository;
            this.log = log;
            this.updateStageContractValidator = updateStageContractValidator;
            this.createStageContractValidator = createStageContractValidator;
            this.processStatusContractValidator = processStatusContractValidator;
        }

        public CreatedStageContract Create(CreateStageContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Status}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Status}");
            var stage = this.mapper.Map<Stage>(contract);

            var createdStage = this.processStageRepository.Create(stage);

            this.log.LogInformation($"Complete for {contract.Status}");
            this.unitOfWork.Complete();

            var createdStageContract = this.mapper.Map<CreatedStageContract>(createdStage);

            this.log.LogInformation($"Return {contract.Status}");
            return createdStageContract;
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching Stage {id}");

            var stage = this.ReadStage(id);

            this.log.LogInformation($"Deleting stage {id}");

            this.processStageRepository.Delete(stage);

            this.unitOfWork.Complete();
        }

        public IEnumerable<ReadedStageContract> List()
        {
            var stageQuery = this.processStageRepository
                .QueryEager();

            var stageResult = stageQuery.ToList();

            return this.mapper.Map<List<ReadedStageContract>>(stageResult);
        }

        public ReadedStageContract Read(int id)
        {
            var stageQuery = this.ReadStage(id);

            return this.mapper.Map<ReadedStageContract>(stageQuery);
        }

        public void Update(UpdateStageContract contract)
        {
            this.ValidateContract(contract);
            var stage = this.mapper.Map<Stage>(contract);
            var updatedStage = this.UpdateStage(stage);
            this.unitOfWork.Complete();
        }

        public CreatedStageItemContract AddItemToStage(CreateStageItemContract createStageItemContract)
        {
            var stageItem = this.mapper.Map<StageItem>(createStageItemContract);

            var createdStageItem = this.stageItemRepository.Create(stageItem);

            var createdStageContract = this.mapper.Map<CreatedStageItemContract>(createdStageItem);

            return createdStageContract;
        }

        public void RemoveItemToStage(int stageItemId)
        {
            this.log.LogInformation($"Searching StageItem {stageItemId}");

            var stageItem = this.stageItemRepository.Query().FirstOrDefault(x => x.Id == stageItemId);

            this.log.LogInformation($"Deleting stageItem {stageItemId}");

            this.stageItemRepository.Delete(stageItem);

            this.unitOfWork.Complete();
        }

        public void UpdateStageItem(UpdateStageItemContract updateStageItemContract)
        {
            var stageItem = this.mapper.Map<StageItem>(updateStageItemContract);

            var updatedStageItem = this.stageItemRepository.Update(stageItem);

            this.unitOfWork.Complete();
        }

        private Stage UpdateStage(Stage stage)
        {
            var existingStage = this.ReadStage(stage.Id);

            if (existingStage != null)
            {
                this.processStageRepository.UpdateStage(stage, existingStage);
            }

            return stage;
        }

        private Stage ReadStage(int id)
        {
            var stageQuery = this.processStageRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            return stageQuery.SingleOrDefault();
        }

        private void ValidateContract(CreateStageContract contract)
        {
            try
            {
                this.createStageContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");

                var process = this.GetProcess(contract.ProcessId);
                var processContract = this.mapper.Map<ReadedProcessContract>(process);
                this.processStatusContractValidator.ValidateAndThrow(processContract);
            }
            catch (ValidationException ex)
            {
                throw new CreateStageInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateStageContract contract)
        {
            try
            {
                this.updateStageContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETDEFAULT}");

                var process = this.GetProcess(contract.ProcessId);
                var processContract = this.mapper.Map<ReadedProcessContract>(process);
                this.processStatusContractValidator.ValidateAndThrow(processContract);
            }
            catch (ValidationException ex)
            {
                throw new UpdateStageInvalidException(ex.ToListOfMessages());
            }
        }

        private Process GetProcess(int processId)
        {
            var process = this.processRepository.Query().Where(p => p.Id == processId).FirstOrDefault();
            if (process == null)
            {
                throw new ValidationException("Process was not found");
            }

            return process;
        }
    }
}
