// <copyright file="TaskService.cs" company="Softvision">
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
    using Domain.Model.Enum;
    using Domain.Model.Exceptions.Task;
    using Domain.Services.Contracts.Task;
    using Domain.Services.Impl.Validators;
    using Domain.Services.Impl.Validators.Task;
    using Domain.Services.Interfaces.Services;
    using FluentValidation;

    public class TaskService : ITaskService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Task> taskRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILog<TaskService> log;
        private readonly UpdateTaskContractValidator updateTaskContractValidator;
        private readonly CreateTaskContractValidator createTaskContractValidator;
        private readonly IRepository<User> userRepository;

        public TaskService(
            IMapper mapper,
            IRepository<Task> taskRepository,
            IUnitOfWork unitOfWork,
            ILog<TaskService> log,
            IRepository<User> userRepository,
            UpdateTaskContractValidator updateTaskContractValidator,
            CreateTaskContractValidator createTaskContractValidator)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.taskRepository = taskRepository;
            this.log = log;
            this.updateTaskContractValidator = updateTaskContractValidator;
            this.createTaskContractValidator = createTaskContractValidator;
            this.userRepository = userRepository;
        }

        public CreatedTaskContract Create(CreateTaskContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Title}");
            this.ValidateContract(contract);

            this.log.LogInformation($"Mapping contract {contract.Title}");
            var task = this.mapper.Map<Task>(contract);

            var createdTask = this.taskRepository.Create(task);
            this.log.LogInformation($"Complete for {contract.Title}");
            this.unitOfWork.Complete();
            this.log.LogInformation($"Return {contract.Title}");
            return this.mapper.Map<CreatedTaskContract>(createdTask);
        }

        public void Delete(int id)
        {
            this.log.LogInformation($"Searching task {id}");
            Task task = this.taskRepository.QueryEager().Where(_ => _.Id == id).FirstOrDefault();

            if (task == null)
            {
                throw new DeleteTaskNotFoundException(id);
            }

            this.log.LogInformation($"Deleting task {id}");
            this.taskRepository.Delete(task);

            this.unitOfWork.Complete();
        }

        public void Update(UpdateTaskContract contract)
        {
            this.log.LogInformation($"Validating contract {contract.Title}");
            this.ValidateContract(contract);

            // Update isApprove property if necessary
            this.UpdateApprovalIfNecessary(contract);

            this.log.LogInformation($"Mapping contract {contract.Title}");
            var task = this.mapper.Map<Task>(contract);

            var updatedTask = this.taskRepository.Update(task);
            this.log.LogInformation($"Complete for {contract.Title}");
            this.unitOfWork.Complete();
        }

        private void UpdateApprovalIfNecessary(UpdateTaskContract contract)
        {
            if (contract.TaskItems.Count > 0)
            {
                var shouldBeApproved = contract.TaskItems.All(c => c.Checked == true);
                contract.IsApprove = shouldBeApproved;
                contract.IsNew = false;
            }
        }

        public void Approve(int id)
        {
            var taskResult = this.taskRepository.QueryEager().Where(_ => _.Id == id).SingleOrDefault();

            taskResult.IsApprove = true;
            taskResult.IsNew = false;

            if (taskResult.TaskItems != null && taskResult.TaskItems.Count > 0)
            {
                foreach (var item in taskResult.TaskItems)
                {
                    item.Checked = true;
                }
            }

            this.unitOfWork.Complete();
        }

        public ReadedTaskContract Read(int id)
        {
            var taskQuery = this.taskRepository
                .QueryEager()
                .Where(_ => _.Id == id)
                .OrderBy(_ => _.Title)
                .ThenBy(_ => _.CreationDate);

            var taskResult = taskQuery.SingleOrDefault();

            return this.mapper.Map<ReadedTaskContract>(taskResult);
        }

        public IEnumerable<ReadedTaskContract> List(int id)
        {
            var role = this.userRepository.QueryEager().Where(x => x.Id == id).FirstOrDefault().Role;
            var taskQuery = new List<Task>();
            if (role == Roles.HRManagement || role == Roles.Admin || role == Roles.Recruiter)
            {
                taskQuery = this.taskRepository
                .QueryEager()
                .OrderBy(_ => _.Title)
                .ThenBy(_ => _.CreationDate)
                .ToList();
            }
            else
            {
                taskQuery = this.taskRepository
                .QueryEager()
                .Where(x => x.UserId == id)
                .OrderBy(_ => _.Title)
                .ThenBy(_ => _.CreationDate)
                .ToList();
            }

            return this.mapper.Map<List<ReadedTaskContract>>(taskQuery);
        }

        public IEnumerable<ReadedTaskContract> ListByUser(string userEmail)
        {
            var taskQuery = this.taskRepository
                .QueryEager()
                .Where(_ => _.User.Username.ToLower() == userEmail.ToLower())
                .OrderBy(_ => _.Id)
                .ThenBy(_ => _.CreationDate);

            var taskResult = taskQuery.ToList();

            return this.mapper.Map<List<ReadedTaskContract>>(taskResult);
        }

        private void ValidateContract(CreateTaskContract contract)
        {
            try
            {
                this.createTaskContractValidator.ValidateAndThrow(
                    contract,
                    $"{ValidatorConstants.RULESETCREATE}");
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
                this.updateTaskContractValidator.ValidateAndThrow(
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
