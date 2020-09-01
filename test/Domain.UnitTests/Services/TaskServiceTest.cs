// <copyright file="TaskServiceTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Core;
    using Core.Persistance;
    using Domain.Model;
    using Domain.Services.Contracts.Task;
    using Domain.Services.Contracts.TaskItem;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.Task;
    using Domain.Services.Repositories.EF;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class TaskServiceTest : BaseDomainTest
    {
        private readonly TaskService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Task>> mockRepositoryTask;
        private readonly Mock<IRepository<TaskItem>> mockRepositoryTaskItem;
        private readonly Mock<ILog<TaskService>> mockLogTaskService;
        private readonly Mock<UpdateTaskContractValidator> mockUpdateTaskContractValidator;
        private readonly Mock<CreateTaskContractValidator> mockCreateTaskContractValidator;
        private readonly Mock<IRepository<User>> userRepository;

        public TaskServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryTask = new Mock<IRepository<Task>>();
            this.mockRepositoryTaskItem = new Mock<IRepository<TaskItem>>();
            this.mockLogTaskService = new Mock<ILog<TaskService>>();
            this.mockUpdateTaskContractValidator = new Mock<UpdateTaskContractValidator>();
            this.mockCreateTaskContractValidator = new Mock<CreateTaskContractValidator>();
            this.userRepository = new Mock<IRepository<User>>();
            this.service = new TaskService(this.mockMapper.Object, this.mockRepositoryTask.Object, this.MockUnitOfWork.Object, this.mockLogTaskService.Object, this.userRepository.Object, this.mockUpdateTaskContractValidator.Object, this.mockCreateTaskContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create TaskService when data is valid")]
        public void Should_CreateTaskService_When_DataIsValid()
        {
            var contract = new CreateTaskContract();
            var expectedTask = new CreatedTaskContract();
            this.mockCreateTaskContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>())).Returns(expectedTask);

            var createdTask = this.service.Create(contract);

            Assert.NotNull(createdTask);
            Assert.Equal(expectedTask, createdTask);
            this.mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateTaskContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<CreateTaskContract>()), Times.Once);
            this.mockRepositoryTask.Verify(mrt => mrt.Create(It.IsAny<Task>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateTaskContract();
            var expectedTask = new CreatedTaskContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateTaskContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>())).Returns(expectedTask);

            var exception = Assert.Throws<Model.Exceptions.Task.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateTaskContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<CreateTaskContract>()), Times.Never);
            this.mockRepositoryTask.Verify(mrt => mrt.Create(It.IsAny<Task>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete TaskService when data is valid")]
        public void Should_DeleteTaskService_When_DataIsValid()
        {
            var id = 0;
            var tasks = new List<Task>() { new Task() { Id = id } }.AsQueryable();
            this.mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);

            this.service.Delete(id);

            this.mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryTask.Verify(mrt => mrt.QueryEager(), Times.Once);
            this.mockRepositoryTask.Verify(mrt => mrt.Delete(It.IsAny<Task>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for deletion is invalid")]
        public void Should_ThrowDeleteTaskNotFoundException_When_DataIsInvalid()
        {
            var id = 0;
            var expectedErrorMEssage = $"Task not found for the TaskId: {id}";

            var exception = Assert.Throws<Model.Exceptions.Task.DeleteTaskNotFoundException>(() => this.service.Delete(id));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryTask.Verify(mrt => mrt.QueryEager(), Times.Once);
            this.mockRepositoryTask.Verify(mrt => mrt.Delete(It.IsAny<Task>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update TaskService when data is valid (it is not approved and new)")]
        public void Should_UpdateTaskService_When_DataIsValidNotApprovedAndNew()
        {
            var contract = new UpdateTaskContract()
            {
                IsNew = true,
                TaskItems = new List<CreateTaskItemContract>(),
            };
            this.mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            this.service.Update(contract);

            Assert.False(contract.IsApprove);
            Assert.True(contract.IsNew);
            this.mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Once);
            this.mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update TaskService when data is valid (it's approved but it isn't new)")]
        public void Should_UpdateTaskService_When_DataIsValidApprovedAndNotNew()
        {
            var contract = new UpdateTaskContract()
            {
                IsNew = true,
                TaskItems = new List<CreateTaskItemContract>()
                {
                    new CreateTaskItemContract() { Checked = true },
                },
            };
            this.mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            this.service.Update(contract);

            Assert.True(contract.IsApprove);
            Assert.False(contract.IsNew);
            this.mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Once);
            this.mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for updating is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_DataIsInvalid()
        {
            var contract = new UpdateTaskContract()
            {
                TaskItems = new List<CreateTaskItemContract>(),
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            var exception = Assert.Throws<Model.Exceptions.Task.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Never);
            this.mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var tasks = new List<Task>() { new Task() { Id = 1 } }.AsQueryable();
            var readedTaksContract = new List<ReadedTaskContract> { new ReadedTaskContract { Id = 1 } };
            this.mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedTaskContract>>(It.IsAny<List<Task>>())).Returns(readedTaksContract);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryTask.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedTaskContract>>(It.IsAny<List<Task>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that listByUser returns a value")]
        public void GivenListByUser_WhenRegularCall_ReturnsValue()
        {
            var tasks = new List<Task>() { new Task() { Id = 1, User = new User { Username = "Email" } } }.AsQueryable();
            var readedTaksContract = new List<ReadedTaskContract> { new ReadedTaskContract { Id = 1 } };
            this.mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedTaskContract>>(It.IsAny<List<Task>>())).Returns(readedTaksContract);

            var actualResult = this.service.ListByUser("Email");

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryTask.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedTaskContract>>(It.IsAny<List<Task>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var tasks = new List<Task>() { new Task() { Id = 1 } }.AsQueryable();
            var readedTaksContract = new ReadedTaskContract { Id = 1 };
            this.mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);
            this.mockMapper.Setup(mm => mm.Map<ReadedTaskContract>(It.IsAny<Task>())).Returns(readedTaksContract);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.Id);
            this.mockRepositoryTask.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedTaskContract>(It.IsAny<Task>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that Approve runs correctly")]
        public void GivenApprove_WhenRegularCall_RunsCorrectly()
        {
            var taskItems = new List<TaskItem> { new TaskItem() };
            var tasks = new List<Task>() { new Task() { Id = 1, TaskItems = taskItems } }.AsQueryable();
            this.mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);

            this.service.Approve(1);

            this.mockRepositoryTask.Verify(_ => _.QueryEager(), Times.Once);
            this.MockUnitOfWork.Verify(x => x.Complete(), Times.Once);
        }
    }
}