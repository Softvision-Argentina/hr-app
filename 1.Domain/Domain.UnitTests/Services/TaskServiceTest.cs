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
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class TaskServiceTest : BaseDomainTest
    {
        private readonly TaskService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Task>> mockRepositoryTask;
        private readonly Mock<IRepository<TaskItem>> mockRepositoryTaskItem;
        private readonly Mock<ILog<TaskService>> mockLogTaskService;
        private readonly Mock<UpdateTaskContractValidator> mockUpdateTaskContractValidator;
        private readonly Mock<CreateTaskContractValidator> mockCreateTaskContractValidator;

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

        [Fact(DisplayName = "Verify that throws error when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateTaskContract();
            var expectedTask = new CreatedTaskContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateTaskContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>())).Returns(expectedTask);

            var exception = Assert.Throws<Model.Exceptions.Task.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateTaskContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<CreateTaskContract>()), Times.Never);
            mockRepositoryTask.Verify(mrt => mrt.Create(It.IsAny<Task>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete TaskService when data is valid")]
        public void Should_DeleteTaskService_When_DataIsValid()
        {
            var id = 0;
            var tasks = new List<Task>() { new Task() { Id = id } }.AsQueryable();
            mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);

            service.Delete(id);

            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryTask.Verify(mrt => mrt.QueryEager(), Times.Once);
            mockRepositoryTask.Verify(mrt => mrt.Delete(It.IsAny<Task>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for deletion is invalid")]
        public void Should_ThrowDeleteTaskNotFoundException_When_DataIsInvalid()
        {
            var id = 0;
            var expectedErrorMEssage = $"Task not found for the TaskId: {id}";

            var exception = Assert.Throws<Model.Exceptions.Task.DeleteTaskNotFoundException>(() => service.Delete(id));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryTask.Verify(mrt => mrt.QueryEager(), Times.Once);
            mockRepositoryTask.Verify(mrt => mrt.Delete(It.IsAny<Task>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update TaskService when data is valid (it is not approved and new)")]
        public void Should_UpdateTaskService_When_DataIsValidNotApprovedAndNew()
        {
            var contract = new UpdateTaskContract()
            {
                IsNew = true,
                TaskItems = new List<CreateTaskItemContract>()
            };
            mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            service.Update(contract);

            Assert.False(contract.IsApprove);
            Assert.True(contract.IsNew);
            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Once);
            mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update TaskService when data is valid (it's approved but it isn't new)")]
        public void Should_UpdateTaskService_When_DataIsValidApprovedAndNotNew()
        {
            var contract = new UpdateTaskContract()
            {
                IsNew = true,
                TaskItems = new List<CreateTaskItemContract>()
                {
                    new CreateTaskItemContract(){ Checked = true }
                }
            };
            mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            service.Update(contract);

            Assert.True(contract.IsApprove);
            Assert.False(contract.IsNew);
            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Once);
            mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for updating is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_DataIsInvalid()
        {
            var contract = new UpdateTaskContract()
            {
                TaskItems = new List<CreateTaskItemContract>()
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            var exception = Assert.Throws<Model.Exceptions.Task.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Never);
            mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }
    }
}