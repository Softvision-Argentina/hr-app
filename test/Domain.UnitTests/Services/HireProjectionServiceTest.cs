using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.HireProjection;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.HireProjection;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class HireProjectionServiceTest : BaseDomainTest
    {
        private readonly HireProjectionService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<HireProjection>> _mockRepositoryHireProjection;                
        private readonly Mock<ILog<HireProjectionService>> _mockLogHireProjectionService;
        private readonly Mock<UpdateHireProjectionContractValidator> _mockUpdateHireProjectionContractValidator;
        private readonly Mock<CreateHireProjectionContractValidator> _mockCreateHireProjectionContractValidator;

        public HireProjectionServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryHireProjection = new Mock<IRepository<HireProjection>>();                        
            _mockLogHireProjectionService = new Mock<ILog<HireProjectionService>>();
            _mockUpdateHireProjectionContractValidator = new Mock<UpdateHireProjectionContractValidator>();
            _mockCreateHireProjectionContractValidator = new Mock<CreateHireProjectionContractValidator>();
            _service = new HireProjectionService(
                _mockMapper.Object,
                _mockRepositoryHireProjection.Object,                
                MockUnitOfWork.Object,
                _mockLogHireProjectionService.Object,
                _mockUpdateHireProjectionContractValidator.Object,
                _mockCreateHireProjectionContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create HireProjectionService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateHireProjectionService()
        {
            var contract = new CreateHireProjectionContract();
            var expectedHireProjection = new CreatedHireProjectionContract();
            _mockCreateHireProjectionContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>())).Returns(new HireProjection());
            _mockRepositoryHireProjection.Setup(repoCom => repoCom.Create(It.IsAny<HireProjection>())).Returns(new HireProjection());
            _mockMapper.Setup(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(expectedHireProjection);

            var createdHireProjection = _service.Create(contract);

            Assert.NotNull(createdHireProjection);
            Assert.Equal(expectedHireProjection, createdHireProjection);
            _mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateHireProjectionContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>()), Times.Once);
            _mockRepositoryHireProjection.Verify(mrt => mrt.Create(It.IsAny<HireProjection>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateHireProjectionContract();
            var expectedHireProjection = new CreatedHireProjectionContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateHireProjectionContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(expectedHireProjection);

            var exception = Assert.Throws<Model.Exceptions.HireProjection.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateHireProjectionContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>()), Times.Never);
            _mockRepositoryHireProjection.Verify(mrt => mrt.Create(It.IsAny<HireProjection>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete HireProjectionService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteHireProjectionService()
        {
            var HireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            _mockRepositoryHireProjection.Setup(mrt => mrt.Query()).Returns(HireProjections);

            _service.Delete(1);

            _mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryHireProjection.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryHireProjection.Verify(mrt => mrt.Delete(It.IsAny<HireProjection>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteHireProjectionNotFoundException()
        {
            var expectedErrorMEssage = $"Hire projection not found for the hireProjectionId: {0}";

            var exception = Assert.Throws<Model.Exceptions.HireProjection.DeleteHireProjectionNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryHireProjection.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryHireProjection.Verify(mrt => mrt.Delete(It.IsAny<HireProjection>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update HireProjectionService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateHireProjectionContract();
            _mockUpdateHireProjectionContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>())).Returns(new HireProjection());

            _service.Update(contract);

            _mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateHireProjectionContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>()), Times.Once);
            _mockRepositoryHireProjection.Verify(mrt => mrt.Update(It.IsAny<HireProjection>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateHireProjectionContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateHireProjectionContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>())).Returns(new HireProjection());

            var exception = Assert.Throws<Model.Exceptions.HireProjection.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateHireProjectionContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>()), Times.Never);
            _mockRepositoryHireProjection.Verify(mrt => mrt.Update(It.IsAny<HireProjection>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var HireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            var readedHireProjectionList = new List<ReadedHireProjectionContract> { new ReadedHireProjectionContract { Id = 1 } };
            _mockRepositoryHireProjection.Setup(mrt => mrt.QueryEager()).Returns(HireProjections);
            _mockMapper.Setup(mm => mm.Map<List<ReadedHireProjectionContract>>(It.IsAny<List<HireProjection>>())).Returns(readedHireProjectionList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryHireProjection.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedHireProjectionContract>>(It.IsAny<List<HireProjection>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var HireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            var readedHireProjection = new ReadedHireProjectionContract { Id = 1 };
            _mockRepositoryHireProjection.Setup(mrt => mrt.QueryEager()).Returns(HireProjections);
            _mockMapper.Setup(mm => mm.Map<ReadedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(readedHireProjection);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedHireProjection, actualResult);
            _mockRepositoryHireProjection.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Once);
        }
    }
}