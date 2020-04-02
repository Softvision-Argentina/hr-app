using AutoMapper;
using Core;
using Domain.Model;
using Domain.Services.Contracts.Process;
using Domain.Services.Contracts.Stage;
using Domain.Services.Contracts.Stage.StageItem;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.Stage;
using Domain.Services.Interfaces.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class ProcessStageServiceTest : BaseDomainTest
    {
        private readonly ProcessStageService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IProcessStageRepository> mockRepositoryProcessStage;
        private readonly Mock<IStageItemRepository> mockRepositoryStageItem;
        private readonly Mock<IProcessRepository> mockRepositoryProcess;
        private readonly Mock<ILog<ProcessStageService>> mockLogProcessStageService;
        private readonly Mock<UpdateStageContractValidator> mockUpdateProcessStageContractValidator;
        private readonly Mock<CreateStageContractValidator> mockCreateStageContractValidator;
        private readonly Mock<ProcessStatusContractValidator> mockProcessStatusContractValidator;

        public ProcessStageServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryProcessStage = new Mock<IProcessStageRepository>();
            mockRepositoryStageItem = new Mock<IStageItemRepository>();
            mockRepositoryProcess = new Mock<IProcessRepository>();            
            mockLogProcessStageService = new Mock<ILog<ProcessStageService>>();
            mockUpdateProcessStageContractValidator = new Mock<UpdateStageContractValidator>();
            mockCreateStageContractValidator = new Mock<CreateStageContractValidator>();
            mockProcessStatusContractValidator = new Mock<ProcessStatusContractValidator>();
            service = new ProcessStageService(
                mockMapper.Object,
                mockRepositoryProcessStage.Object,
                mockRepositoryStageItem.Object,
                mockRepositoryProcess.Object,
                MockUnitOfWork.Object,
                mockLogProcessStageService.Object,
                mockUpdateProcessStageContractValidator.Object,
                mockCreateStageContractValidator.Object,
                mockProcessStatusContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create ProcessStageService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateProcessStageService()
        {
            var contract = new CreateStageContract();
            var expectedProcessStage = new CreatedStageContract();
            var processes = new List<Process> { new Process { Id = 0 } }.AsQueryable();
            mockCreateStageContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>())).Returns(new Stage());
            mockRepositoryProcessStage.Setup(repoCom => repoCom.Create(It.IsAny<Stage>())).Returns(new Stage());
            mockRepositoryProcess.Setup(x => x.Query()).Returns(processes);
            mockMapper.Setup(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>())).Returns(expectedProcessStage);
            mockProcessStatusContractValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ReadedProcessContract>>())).Returns(new ValidationResult());

            var createdProcessStage = service.Create(contract);

            Assert.NotNull(createdProcessStage);
            Assert.Equal(expectedProcessStage, createdProcessStage);
            mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateStageContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>()), Times.Once);
            mockRepositoryProcessStage.Verify(mrt => mrt.Create(It.IsAny<Stage>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateStageContract();
            var expectedProcessStage = new CreatedStageContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateStageContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>())).Returns(expectedProcessStage);

            var exception = Assert.Throws<Model.Exceptions.Stage.CreateStageInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateStageContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateStageContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<CreateStageContract>()), Times.Never);
            mockRepositoryProcessStage.Verify(mrt => mrt.Create(It.IsAny<Stage>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedStageContract>(It.IsAny<Stage>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete ProcessStageService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteProcessStageService()
        {
            var ProcessStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(ProcessStages);

            service.Delete(1);

            mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryProcessStage.Verify(mrt => mrt.QueryEager(), Times.Once);
            mockRepositoryProcessStage.Verify(mrt => mrt.Delete(It.IsAny<Stage>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update ProcessStageService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateStageContract();
            var processes = new List<Process> { new Process { Id = 0 } }.AsQueryable();
            mockUpdateProcessStageContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>())).Returns(new Stage());
            mockRepositoryProcess.Setup(x => x.Query()).Returns(processes);
            mockProcessStatusContractValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<ReadedProcessContract>>())).Returns(new ValidationResult());

            service.Update(contract);
            
            mockUpdateProcessStageContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>()), Times.Once);            
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateStageContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateProcessStageContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>())).Returns(new Stage());

            var exception = Assert.Throws<Model.Exceptions.Stage.UpdateStageInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);            
            mockUpdateProcessStageContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateStageContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Stage>(It.IsAny<UpdateStageContract>()), Times.Never);
            mockRepositoryProcessStage.Verify(mrt => mrt.Update(It.IsAny<Stage>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var ProcessStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            var readedProcessStageList = new List<ReadedStageContract> { new ReadedStageContract { Id = 1 } };
            mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(ProcessStages);
            mockMapper.Setup(mm => mm.Map<List<ReadedStageContract>>(It.IsAny<List<Stage>>())).Returns(readedProcessStageList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryProcessStage.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedStageContract>>(It.IsAny<List<Stage>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var ProcessStages = new List<Stage>() { new Stage() { Id = 1 } }.AsQueryable();
            var readedProcessStage = new ReadedStageContract { Id = 1 };
            mockRepositoryProcessStage.Setup(mrt => mrt.QueryEager()).Returns(ProcessStages);
            mockMapper.Setup(mm => mm.Map<ReadedStageContract>(It.IsAny<Stage>())).Returns(readedProcessStage);

            var actualResult = service.Read(1);
            
            Assert.NotNull(actualResult);
            Assert.Equal(readedProcessStage, actualResult);
            mockRepositoryProcessStage.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedStageContract>(It.IsAny<Stage>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that AddItemToStage returns a value")]
        public void GivenAddItemToStage_WhenRegularCall_ReturnsValue()
        {
            var ProcessStages = new List<Stage>().AsQueryable();            
            var createdStageItemContract = new CreatedStageItemContract { Id = 1 };            
            mockMapper.Setup(mm => mm.Map<StageItem>(It.IsAny<CreateStageItemContract>())).Returns(new StageItem());
            mockMapper.Setup(mm => mm.Map<CreatedStageItemContract>(It.IsAny<StageItem>())).Returns(createdStageItemContract);

            var actualResult = service.AddItemToStage(new CreateStageItemContract());

            Assert.NotNull(actualResult);
            Assert.Equal(actualResult.Id, createdStageItemContract.Id);            
            mockMapper.Verify(_ => _.Map<StageItem>(It.IsAny<CreateStageItemContract>()), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedStageItemContract>(It.IsAny<StageItem>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that RemoveItemToStage runs")]
        public void GivenRemoveItemToStage_WhenRegularCall_RemoveItem()
        {
            mockMapper.Setup(mm => mm.Map<StageItem>(It.IsAny<CreateStageItemContract>())).Returns(new StageItem());
            mockMapper.Setup(mm => mm.Map<CreatedStageItemContract>(It.IsAny<StageItem>())).Returns(new CreatedStageItemContract());

            service.RemoveItemToStage(1);

            mockLogProcessStageService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryStageItem.Verify(x => x.Delete(It.IsAny<StageItem>()), Times.Exactly(1));
            MockUnitOfWork.Verify(x => x.Complete(), Times.Exactly(1));
        }
    }
}