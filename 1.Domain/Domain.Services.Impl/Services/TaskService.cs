using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Task;
using Domain.Services.Contracts.Task;
using Domain.Services.Impl.Validators;
using Domain.Services.Impl.Validators.Task;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Services.Impl.Services
{
    public class TaskService: ITaskService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<TaskItem> _taskItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<TaskService> _log;
        private readonly UpdateTaskContractValidator _updateTaskContractValidator;
        private readonly CreateTaskContractValidator _createTaskContractValidator;

        public TaskService(IMapper mapper,
            IRepository<Task> taskRepository,
            IRepository<TaskItem> taskItemRepository,
            IUnitOfWork unitOfWork,
            ILog<TaskService> log,
            UpdateTaskContractValidator updateTaskContractValidator,
            CreateTaskContractValidator createTaskContractValidator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _taskItemRepository = taskItemRepository;
            _taskRepository = taskRepository;
            _log = log;
            _updateTaskContractValidator = updateTaskContractValidator;
            _createTaskContractValidator = createTaskContractValidator;
        }

        public CreatedTaskContract Create(CreateTaskContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Title}");
            ValidateContract(contract);

            _log.LogInformation($"Mapping contract {contract.Title}");
            var task = _mapper.Map<Task>(contract);

            var createdTask = _taskRepository.Create(task);
            _log.LogInformation($"Complete for {contract.Title}");
            _unitOfWork.Complete();
            _log.LogInformation($"Return {contract.Title}");
            return _mapper.Map<CreatedTaskContract>(createdTask);
        }

        public void Delete(int id)
        {
            _log.LogInformation($"Searching task {id}");
            Task task = _taskRepository.QueryEager().Where(_ => _.Id == id).FirstOrDefault();

            if (task == null)
            {
                throw new DeleteTaskNotFoundException(id);
            }
            _log.LogInformation($"Deleting task {id}");
            _taskRepository.Delete(task);

            _unitOfWork.Complete();
        }

        public void Update(UpdateTaskContract contract)
        {
            _log.LogInformation($"Validating contract {contract.Title}");
            ValidateContract(contract);

            //Update isApprove property if necessary
            UpdateApprovalIfNecessary(contract);

            _log.LogInformation($"Mapping contract {contract.Title}");
            var task = _mapper.Map<Task>(contract);

            var updatedTask = _taskRepository.Update(task);
            _log.LogInformation($"Complete for {contract.Title}");
            _unitOfWork.Complete();
        }

        private void UpdateApprovalIfNecessary(UpdateTaskContract contract)
        {
            if(contract.TaskItems.Count > 0)
            {
                var shouldBeApproved = contract.TaskItems.All(c => c.Checked == true);
                contract.IsApprove = shouldBeApproved;
                contract.IsNew = false;
            }

        }

        public void Approve(int id)
        {
            var taskResult = _taskRepository.QueryEager().Where(_ => _.Id == id).SingleOrDefault();

            taskResult.IsApprove = true;
            taskResult.IsNew = false;

            if(taskResult.TaskItems != null && taskResult.TaskItems.Count > 0)
                foreach (var item in taskResult.TaskItems)
                {
                    item.Checked = true;
                }

            _unitOfWork.Complete();
        }

        public ReadedTaskContract Read(int id)
        {
            var taskQuery = _taskRepository
                .QueryEager()
                .Where(_ => _.Id == id)
                .OrderBy(_ => _.Title)
                .ThenBy(_ => _.CreationDate);

            var taskResult = taskQuery.SingleOrDefault();

            return _mapper.Map<ReadedTaskContract>(taskResult);
        }



        public IEnumerable<ReadedTaskContract> List()
        {
            var taskQuery = _taskRepository
                .QueryEager()
                .OrderBy(_ => _.Title)
                .ThenBy(_ => _.CreationDate);

            var taskResult = taskQuery.ToList();

            return _mapper.Map<List<ReadedTaskContract>>(taskResult);
        }

        public IEnumerable<ReadedTaskContract> ListByUser(string userEmail)
        {
            var taskQuery = _taskRepository
                .QueryEager()
                .Where(_ => _.User.Username.ToLower() == userEmail.ToLower())
                .OrderBy(_ => _.Id)
                .ThenBy(_ => _.CreationDate);

            var taskResult = taskQuery.ToList();

            return _mapper.Map<List<ReadedTaskContract>>(taskResult);
        }

        private void ValidateContract(CreateTaskContract contract)
        {
            try
            {
                _createTaskContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_CREATE}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }

        private void ValidateContract(UpdateTaskContract contract)
        {
            try
            {
                _updateTaskContractValidator.ValidateAndThrow(contract,
                    $"{ValidatorConstants.RULESET_DEFAULT}");
            }
            catch (ValidationException ex)
            {
                throw new CreateContractInvalidException(ex.ToListOfMessages());
            }
        }
    }
}
