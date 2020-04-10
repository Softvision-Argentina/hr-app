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
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class ProcessStageService : IProcessStageService
    {
        private readonly IMapper _mapper;
        private readonly IProcessStageRepository _processStageRepository;
        private readonly IStageItemRepository _stageItemRepository;
        private readonly IProcessRepository _processRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<ProcessStageService> _log;
        private readonly UpdateStageContractValidator _updateStageContractValidator;
        private readonly CreateStageContractValidator _createStageContractValidator;
        private readonly ProcessStatusContractValidator _processStatusContractValidator;

        public ProcessStageService(
            IMapper mapper,
            IProcessStageRepository processStageRepository,
            IStageItemRepository stageItemRepository,
            IProcessRepository processRepository,
            IUnitOfWork unitOfWork,
            ILog<ProcessStageService> log,
            UpdateStageContractValidator updateStageContractValidator,
            CreateStageContractValidator createStageContractValidator,
            ProcessStatusContractValidator processStatusContractValidator
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _processStageRepository = processStageRepository;
            _stageItemRepository = stageItemRepository;
            _processRepository = processRepository;
            _log = log;
            _updateStageContractValidator = updateStageContractValidator;
            _createStageContractValidator = createStageContractValidator;
            _processStatusContractValidator = processStatusContractValidator;
        }

        public CreatedStageContract Create(CreateStageContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Status}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Status}");
            var stage = _mapper.Map<Stage>(contract);

            var createdStage = _processStageRepository.Create(stage);

            _log.LogInformation($"Complete for {contract.Status}");
            _unitOfWork.Complete();

            var createdStageContract = _mapper.Map<CreatedStageContract>(createdStage);

            _log.LogInformation($"Return {contract.Status}");
            return createdStageContract;
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching Stage {id}");

            var stage = ReadStage(id);

            _log.LogInformation($"Deleting stage {id}");

            _processStageRepository.Delete(stage);

            _unitOfWork.Complete();
        }

        public IEnumerable<ReadedStageContract> List()
        {
            var stageQuery = _processStageRepository
                .QueryEager();

            var stageResult = stageQuery.ToList();

            return _mapper.Map<List<ReadedStageContract>>(stageResult);
        }

        public ReadedStageContract Read(int id)
        {
            var stageQuery = ReadStage(id);

            return _mapper.Map<ReadedStageContract>(stageQuery);
        }

        public void Update(UpdateStageContract contract)
        {            
            ValidateContract(contract);         
            var stage = _mapper.Map<Stage>(contract);
            var updatedStage = UpdateStage(stage);
            _unitOfWork.Complete();
        }

        public CreatedStageItemContract AddItemToStage(CreateStageItemContract createStageItemContract)
        {
            var stageItem = _mapper.Map<StageItem>(createStageItemContract);

            var createdStageItem = _stageItemRepository.Create(stageItem);

            var createdStageContract = _mapper.Map<CreatedStageItemContract>(createdStageItem);

            return createdStageContract;
        }

        public void RemoveItemToStage(int stageItemId)
        {
            _log.LogInformation($"Searching StageItem {stageItemId}");

            var stageItem = _stageItemRepository.Query().FirstOrDefault(x => x.Id == stageItemId);

            _log.LogInformation($"Deleting stageItem {stageItemId}");

            _stageItemRepository.Delete(stageItem);

            _unitOfWork.Complete();
        }

        public void UpdateStageItem(UpdateStageItemContract updateStageItemContract)
        {            
            var stageItem = _mapper.Map<StageItem>(updateStageItemContract);

            var updatedStageItem = _stageItemRepository.Update(stageItem);
         
            _unitOfWork.Complete();
        }


        private Stage UpdateStage(Stage stage)
        {
            var existingStage = ReadStage(stage.Id);

            if (existingStage != null)
            {
                _processStageRepository.UpdateStage(stage, existingStage);
            }

            return stage;
        }

        private Stage ReadStage(int id)
        {
            var stageQuery = _processStageRepository
                .QueryEager()
                .Where(_ => _.Id == id);

            return stageQuery.SingleOrDefault();
        }

        private void ValidateContract(CreateStageContract contract)
        {
            try
            {
                _createStageContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");

                var process = this.GetProcess(contract.ProcessId);
                var processContract = _mapper.Map<ReadedProcessContract>(process);
                _processStatusContractValidator.ValidateAndThrow(processContract);
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
                _updateStageContractValidator.ValidateAndThrow(contract,
                   $"{ValidatorConstants.RULESET_DEFAULT}");

                var process = this.GetProcess(contract.ProcessId);
                var processContract = _mapper.Map<ReadedProcessContract>(process);
                _processStatusContractValidator.ValidateAndThrow(processContract);
            }
            catch (ValidationException ex)
            {
                throw new UpdateStageInvalidException(ex.ToListOfMessages());
            }
        }

        private Process GetProcess(int processID)
        {
            var process = this._processRepository.Query().Where(p => p.Id == processID).FirstOrDefault();
            if (process == null)
                throw new ValidationException("Process was not found");
            return process;
        }
    }
}
