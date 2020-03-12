using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Task;
using Domain.Services.Contracts.TaskItem;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.Task;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class TaskServiceTest : BaseDomainTest
    {
        private TaskService service;
        private Mock<IMapper> mockMapper;
        private Mock<IRepository<Task>> mockRepositoryTask;
        private Mock<IRepository<TaskItem>> mockRepositoryTaskItem;
        private Mock<ILog<TaskService>> mockLogTaskService;
        private Mock<UpdateTaskContractValidator> mockUpdateTaskContractValidator;
        private Mock<CreateTaskContractValidator> mockCreateTaskContractValidator;

        public TaskServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryTask = new Mock<IRepository<Task>>();
            mockRepositoryTaskItem = new Mock<IRepository<TaskItem>>();
            mockLogTaskService = new Mock<ILog<TaskService>>();
            mockUpdateTaskContractValidator = new Mock<UpdateTaskContractValidator>();
            mockCreateTaskContractValidator = new Mock<CreateTaskContractValidator>();
            service = new TaskService(mockMapper.Object, mockRepositoryTask.Object, mockRepositoryTaskItem.Object, MockUnitOfWork.Object, mockLogTaskService.Object, mockUpdateTaskContractValidator.Object, mockCreateTaskContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create TaskService when data is valid")]
        public void Should_CreateTaskService_When_DataIsValid()
        {
            var contract = new CreateTaskContract();
            var expectedTask = new CreatedTaskContract();
            //ValidateAndThrow is an extension method therefore we are unable to setup it so we setup the method who is called internaly by ValidateAndThrow
            mockCreateTaskContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>())).Returns(expectedTask);

            var createdTask = service.Create(contract);

            Assert.NotNull(createdTask);
            Assert.Equal(expectedTask, createdTask);
            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateTaskContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<CreateTaskContract>()), Times.Once);
            mockRepositoryTask.Verify(mrt => mrt.Create(It.IsAny<Task>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete TaskService when data is valid")]
        public void Should_DeleteTaskService_When_DataIsValid()
        {
            var id = 0;
            var tasks = new List<Task>() { new Task() { Id = id } } .AsQueryable();
            mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);

            service.Delete(id);

            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryTask.Verify(mrt => mrt.QueryEager(), Times.Once);
            mockRepositoryTask.Verify(mrt => mrt.Delete(It.IsAny<Task>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update TaskService when data is valid")]
        public void Should_UpdateTaskService_When_DataIsValid()
        {
            var contract = new UpdateTaskContract() 
            {
                TaskItems = new List<CreateTaskItemContract>()
            };
            //ValidateAndThrow is an extension method therefore we are unable to setup it so we setup the method who is called internaly by ValidateAndThrow
            mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            service.Update(contract);

            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Once);
            mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }
    }
}
