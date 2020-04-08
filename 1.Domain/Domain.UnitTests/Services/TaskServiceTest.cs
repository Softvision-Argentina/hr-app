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
        private readonly TaskService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Task>> _mockRepositoryTask;
        private readonly Mock<IRepository<TaskItem>> _mockRepositoryTaskItem;
        private readonly Mock<ILog<TaskService>> _mockLogTaskService;
        private readonly Mock<UpdateTaskContractValidator> _mockUpdateTaskContractValidator;
        private readonly Mock<CreateTaskContractValidator> _mockCreateTaskContractValidator;

        public TaskServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryTask = new Mock<IRepository<Task>>();
            _mockRepositoryTaskItem = new Mock<IRepository<TaskItem>>();
            _mockLogTaskService = new Mock<ILog<TaskService>>();
            _mockUpdateTaskContractValidator = new Mock<UpdateTaskContractValidator>();
            _mockCreateTaskContractValidator = new Mock<CreateTaskContractValidator>();
            _service = new TaskService(_mockMapper.Object, _mockRepositoryTask.Object, _mockRepositoryTaskItem.Object, MockUnitOfWork.Object, _mockLogTaskService.Object, _mockUpdateTaskContractValidator.Object, _mockCreateTaskContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create TaskService when data is valid")]
        public void Should_CreateTaskService_When_DataIsValid()
        {
            var contract = new CreateTaskContract();
            var expectedTask = new CreatedTaskContract();
            _mockCreateTaskContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>())).Returns(expectedTask);

            var createdTask = _service.Create(contract);

            Assert.NotNull(createdTask);
            Assert.Equal(expectedTask, createdTask);
            _mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateTaskContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<CreateTaskContract>()), Times.Once);
            _mockRepositoryTask.Verify(mrt => mrt.Create(It.IsAny<Task>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateTaskContract();
            var expectedTask = new CreatedTaskContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateTaskContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>())).Returns(expectedTask);

            var exception = Assert.Throws<Model.Exceptions.Task.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateTaskContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateTaskContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<CreateTaskContract>()), Times.Never);
            _mockRepositoryTask.Verify(mrt => mrt.Create(It.IsAny<Task>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedTaskContract>(It.IsAny<Task>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete TaskService when data is valid")]
        public void Should_DeleteTaskService_When_DataIsValid()
        {
            var id = 0;
            var tasks = new List<Task>() { new Task() { Id = id } }.AsQueryable();
            _mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);

            _service.Delete(id);

            _mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryTask.Verify(mrt => mrt.QueryEager(), Times.Once);
            _mockRepositoryTask.Verify(mrt => mrt.Delete(It.IsAny<Task>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for deletion is invalid")]
        public void Should_ThrowDeleteTaskNotFoundException_When_DataIsInvalid()
        {
            var id = 0;
            var expectedErrorMEssage = $"Task not found for the TaskId: {id}";

            var exception = Assert.Throws<Model.Exceptions.Task.DeleteTaskNotFoundException>(() => _service.Delete(id));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryTask.Verify(mrt => mrt.QueryEager(), Times.Once);
            _mockRepositoryTask.Verify(mrt => mrt.Delete(It.IsAny<Task>()), Times.Never);
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
            _mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            _service.Update(contract);

            Assert.False(contract.IsApprove);
            Assert.True(contract.IsNew);
            _mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Once);
            _mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Once);
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
            _mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            _service.Update(contract);

            Assert.True(contract.IsApprove);
            Assert.False(contract.IsNew);
            _mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Once);
            _mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Once);
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
            _mockUpdateTaskContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>())).Returns(new Task());

            var exception = Assert.Throws<Model.Exceptions.Task.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogTaskService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateTaskContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateTaskContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Task>(It.IsAny<UpdateTaskContract>()), Times.Never);
            _mockRepositoryTask.Verify(mrt => mrt.Update(It.IsAny<Task>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var tasks = new List<Task>() { new Task() { Id = 1 } }.AsQueryable();
            var readedTaksContract = new List<ReadedTaskContract> { new ReadedTaskContract { Id = 1 } };
            _mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);
            _mockMapper.Setup(mm => mm.Map<List<ReadedTaskContract>>(It.IsAny<List<Task>>())).Returns(readedTaksContract);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryTask.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedTaskContract>>(It.IsAny<List<Task>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that listByConsultant returns a value")]
        public void GivenListByConsultant_WhenRegularCall_ReturnsValue()
        {
            var tasks = new List<Task>() { new Task() { Id = 1, Consultant = new Consultant {EmailAddress = "Email" } } }.AsQueryable();
            var readedTaksContract = new List<ReadedTaskContract> { new ReadedTaskContract { Id = 1 } };
            _mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);
            _mockMapper.Setup(mm => mm.Map<List<ReadedTaskContract>>(It.IsAny<List<Task>>())).Returns(readedTaksContract);

            var actualResult = _service.ListByConsultant("Email");

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryTask.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedTaskContract>>(It.IsAny<List<Task>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var tasks = new List<Task>() { new Task() { Id = 1 } }.AsQueryable();
            var readedTaksContract = new ReadedTaskContract { Id = 1 };
            _mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);
            _mockMapper.Setup(mm => mm.Map<ReadedTaskContract>(It.IsAny<Task>())).Returns(readedTaksContract);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(1,actualResult.Id);
            _mockRepositoryTask.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedTaskContract>(It.IsAny<Task>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that Approve runs correctly")]
        public void GivenApprove_WhenRegularCall_RunsCorrectly()
        {
            var taskItems = new List<TaskItem> { new TaskItem() } ;
            var tasks = new List<Task>() { new Task() { Id = 1, TaskItems = taskItems } }.AsQueryable();            
            _mockRepositoryTask.Setup(mrt => mrt.QueryEager()).Returns(tasks);            

            _service.Approve(1);

            _mockRepositoryTask.Verify(_ => _.QueryEager(), Times.Once);
            MockUnitOfWork.Verify(x => x.Complete(), Times.Once);
        }
    }
}