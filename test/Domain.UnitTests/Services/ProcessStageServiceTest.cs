using AutoMapper;
using Core;
using Domain.Model;
using Domain.Services.Contracts.Process;
using Domain.Services.Contracts.Stage;
using Domain.Services.Contracts.Stage.StageItem;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.Stage;
using Domain.Services.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class ProcessStageServiceTest : BaseDomainTest
    {
        private readonly ProcessStageService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IProcessStageRepository> _mockRepositoryProcessStage;
        private readonly Mock<IStageItemRepository> _mockRepositoryStageItem;
        private readonly Mock<IProcessRepository> _mockRepositoryProcess;
        private readonly Mock<ILog<ProcessStageService>> _mockLogProcessStageService;
        private readonly Mock<UpdateStageContractValidator> _mockUpdateProcessStageContractValidator;
        private readonly Mock<CreateStageContractValidator> _mockCreateStageContractValidator;
        private readonly Mock<ProcessStatusContractValidator> _mockProcessStatusContractValidator;

        public ProcessStageServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryProcessStage = new Mock<IProcessStageRepository>();
            _mockRepositoryStageItem = new Mock<IStageItemRepository>();
            _mockRepositoryProcess = new Mock<IProcessRepository>();            
            _mockLogProcessStageService = new Mock<ILog<ProcessStageService>>();
            _mockUpdateProcessStageContractValidator = new Mock<UpdateStageContractValidator>();
            _mockCreateStageContractValidator = new Mock<CreateStageContractValidator>();
            _mockProcessStatusContractValidator = new Mock<ProcessStatusContractValidator>();
            _service = new ProcessStageService(
                _mockMapper.Object,
                _mockRepositoryProcessStage.Object,
                _mockRepositoryStageItem.Object,
                _mockRepositoryProcess.Object,
                MockUnitOfWork.Object,
                _mockLogProcessStageService.Object,
                _mockUpdateProcessStageContractValidator.Object,
                _mockCreateStageContractValidator.Object,
                _mockProcessStatusContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create ProcessStageService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateProcessStageService()
        {
            var contract = new CreateStageContract();
            var expectedProcessStage = new CreatedStageContract();
            var processes = new List<Process> { new Process { Id = 0 } }.AsQueryable();
            _mockCreateStageContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>())).Returns(new Stage());
            _mockRepositoryProcessStage.Setup(repoCom => repoCom.Create(It.IsAny<Stage>())).Returns(new Stage());
            _mockRepositoryProcess.Setup(x => x.Query()).Returns(processes);
            _mockMapper.Setup(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>())).Returns(expectedProcessStage);
            _mockProcessStatusContractValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ReadedProcessContract>>())).Returns(new ValidationResult());

            var createdProcessStage = _service.Create(contract);

            Assert.NotNull(createdProcessStage);
            Assert.Equal(expectedProcessStage, createdProcessStage);
            _mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateStageContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>()), Times.Once);
            _mockRepositoryProcessStage.Verify(mrt => mrt.Create(It.IsAny<Stage>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateStageContract();
            var expectedProcessStage = new CreatedStageContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateStageContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>())).Returns(expectedProcessStage);

            var exception = Assert.Throws<Model.Exceptions.Stage.CreateStageInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateStageContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>()), Times.Never);
            _mockRepositoryProcessStage.Verify(mrt => mrt.Create(It.IsAny<Stage>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete ProcessStageService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteProcessStageService()
        {
            var ProcessStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            _mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(ProcessStages);

            _service.Delete(1);

            _mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryProcessStage.Verify(mrt => mrt.QueryEager(), Times.Once);
            _mockRepositoryProcessStage.Verify(mrt => mrt.Delete(It.IsAny<Stage>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update ProcessStageService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateStageContract();
            var processes = new List<Process> { new Process { Id = 0 } }.AsQueryable();
            _mockUpdateProcessStageContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>())).Returns(new Stage());
            _mockRepositoryProcess.Setup(x => x.Query()).Returns(processes);
            _mockProcessStatusContractValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ReadedProcessContract>>())).Returns(new ValidationResult());

            _service.Update(contract);
            
            _mockUpdateProcessStageContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>()), Times.Once);            
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateStageContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateProcessStageContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>())).Returns(new Stage());

            var exception = Assert.Throws<Model.Exceptions.Stage.UpdateStageInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);            
            _mockUpdateProcessStageContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>()), Times.Never);
            _mockRepositoryProcessStage.Verify(mrt => mrt.Update(It.IsAny<Stage>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var ProcessStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            var readedProcessStageList = new List<ReadedStageContract> { new ReadedStageContract { Id = 1 } };
            _mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(ProcessStages);
            _mockMapper.Setup(mm => mm.Map<List<ReadedStageContract>>(It.IsAny<List<Stage>>())).Returns(readedProcessStageList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryProcessStage.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedStageContract>>(It.IsAny<List<Stage>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var ProcessStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            var readedProcessStage = new ReadedStageContract { Id = 1 };
            _mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(ProcessStages);
            _mockMapper.Setup(mm => mm.Map<ReadedStageContract>(It.IsAny<Stage>())).Returns(readedProcessStage);

            var actualResult = _service.Read(1);
            
            Assert.NotNull(actualResult);
            Assert.Equal(readedProcessStage, actualResult);
            _mockRepositoryProcessStage.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedStageContract>(It.IsAny<Stage>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that AddItemToStage returns a value")]
        public void GivenAddItemToStage_WhenRegularCall_ReturnsValue()
        {
            var ProcessStages = new List<Stage>().AsQueryable();            
            var createdStageItemContract = new CreatedStageItemContract { Id = 1 };            
            _mockMapper.Setup(mm => mm.Map<StageItem>(It.IsAny<CreateStageItemContract>())).Returns(new StageItem());
            _mockMapper.Setup(mm => mm.Map<CreatedStageItemContract>(It.IsAny<StageItem>())).Returns(createdStageItemContract);

            var actualResult = _service.AddItemToStage(new CreateStageItemContract());

            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Id, createdStageItemContract.Id);            
            _mockMapper.Verify(_ => _.Map<StageItem>(It.IsAny<CreateStageItemContract>()), Times.Once);
            _mockMapper.Verify(_ => _.Map<CreatedStageItemContract>(It.IsAny<StageItem>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that RemoveItemToStage runs")]
        public void GivenRemoveItemToStage_WhenRegularCall_RemoveItem()
        {
            _mockMapper.Setup(mm => mm.Map<StageItem>(It.IsAny<CreateStageItemContract>())).Returns(new StageItem());
            _mockMapper.Setup(mm => mm.Map<CreatedStageItemContract>(It.IsAny<StageItem>())).Returns(new CreatedStageItemContract());

            _service.RemoveItemToStage(1);

            _mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryStageItem.Verify(x => x.Delete(It.IsAny<StageItem>()), Times.Exactly(1));
            MockUnitOfWork.Verify(x => x.Complete(), Times.Exactly(1));
        }

        [Fact(DisplayName = "Verify that UpdateStageItem runs")]
        public void GivenUpdateStageItem_WhenRegularCall_RunsCorrectly()
        {
            _mockMapper.Setup(mm => mm.Map<StageItem>(It.IsAny<CreateStageItemContract>())).Returns(new StageItem());                        

            _service.UpdateStageItem(new UpdateStageItemContract());
            
            _mockRepositoryStageItem.Verify(x => x.Update(It.IsAny<StageItem>()), Times.Exactly(1));
            MockUnitOfWork.Verify(x => x.Complete(), Times.Exactly(1));
        }
    }
}