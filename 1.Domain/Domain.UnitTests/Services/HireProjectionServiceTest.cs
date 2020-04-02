using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.HireProjection;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.HireProjection;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class HireProjectionServiceTest : BaseDomainTest
    {
        private readonly HireProjectionService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<HireProjection>> mockRepositoryHireProjection;        
        private readonly Mock<IRepository<Model.HireProjection>> mockRepositoryModelHireProjection;
        private readonly Mock<ILog<HireProjectionService>> mockLogHireProjectionService;
        private readonly Mock<UpdateHireProjectionContractValidator> mockUpdateHireProjectionContractValidator;
        private readonly Mock<CreateHireProjectionContractValidator> mockCreateHireProjectionContractValidator;

        public HireProjectionServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryHireProjection = new Mock<IRepository<HireProjection>>();            
            mockRepositoryModelHireProjection = new Mock<IRepository<Model.HireProjection>>();
            mockLogHireProjectionService = new Mock<ILog<HireProjectionService>>();
            mockUpdateHireProjectionContractValidator = new Mock<UpdateHireProjectionContractValidator>();
            mockCreateHireProjectionContractValidator = new Mock<CreateHireProjectionContractValidator>();
            service = new HireProjectionService(
                mockMapper.Object,
                mockRepositoryHireProjection.Object,                
                MockUnitOfWork.Object,
                mockLogHireProjectionService.Object,
                mockUpdateHireProjectionContractValidator.Object,
                mockCreateHireProjectionContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create HireProjectionService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateHireProjectionService()
        {
            var contract = new CreateHireProjectionContract();
            var expectedHireProjection = new CreatedHireProjectionContract();
            mockCreateHireProjectionContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>())).Returns(new HireProjection());
            mockRepositoryHireProjection.Setup(repoCom => repoCom.Create(It.IsAny<HireProjection>())).Returns(new HireProjection());
            mockMapper.Setup(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(expectedHireProjection);

            var createdHireProjection = service.Create(contract);

            Assert.NotNull(createdHireProjection);
            Assert.Equal(expectedHireProjection, createdHireProjection);
            mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateHireProjectionContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>()), Times.Once);
            mockRepositoryHireProjection.Verify(mrt => mrt.Create(It.IsAny<HireProjection>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateHireProjectionContract();
            var expectedHireProjection = new CreatedHireProjectionContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateHireProjectionContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(expectedHireProjection);

            var exception = Assert.Throws<Model.Exceptions.HireProjection.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateHireProjectionContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateHireProjectionContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<CreateHireProjectionContract>()), Times.Never);
            mockRepositoryHireProjection.Verify(mrt => mrt.Create(It.IsAny<HireProjection>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete HireProjectionService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteHireProjectionService()
        {
            var HireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            mockRepositoryHireProjection.Setup(mrt => mrt.Query()).Returns(HireProjections);

            service.Delete(1);

            mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryHireProjection.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryHireProjection.Verify(mrt => mrt.Delete(It.IsAny<HireProjection>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteHireProjectionNotFoundException()
        {
            var expectedErrorMEssage = $"Hire projection not found for the hireProjectionId: {0}";

            var exception = Assert.Throws<Model.Exceptions.HireProjection.DeleteHireProjectionNotFoundException>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryHireProjection.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryHireProjection.Verify(mrt => mrt.Delete(It.IsAny<HireProjection>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update HireProjectionService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateHireProjectionContract();
            mockUpdateHireProjectionContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>())).Returns(new HireProjection());

            service.Update(contract);

            mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateHireProjectionContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>()), Times.Once);
            mockRepositoryHireProjection.Verify(mrt => mrt.Update(It.IsAny<HireProjection>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateHireProjectionContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateHireProjectionContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>())).Returns(new HireProjection());

            var exception = Assert.Throws<Model.Exceptions.HireProjection.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogHireProjectionService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateHireProjectionContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateHireProjectionContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<HireProjection>(It.IsAny<UpdateHireProjectionContract>()), Times.Never);
            mockRepositoryHireProjection.Verify(mrt => mrt.Update(It.IsAny<HireProjection>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var HireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            var readedHireProjectionList = new List<ReadedHireProjectionContract> { new ReadedHireProjectionContract { Id = 1 } };
            mockRepositoryHireProjection.Setup(mrt => mrt.QueryEager()).Returns(HireProjections);
            mockMapper.Setup(mm => mm.Map<List<ReadedHireProjectionContract>>(It.IsAny<List<HireProjection>>())).Returns(readedHireProjectionList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryHireProjection.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedHireProjectionContract>>(It.IsAny<List<HireProjection>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var HireProjections = new List<HireProjection>() { new HireProjection() { Id = 1 } }.AsQueryable();
            var readedHireProjection = new ReadedHireProjectionContract { Id = 1 };
            mockRepositoryHireProjection.Setup(mrt => mrt.QueryEager()).Returns(HireProjections);
            mockMapper.Setup(mm => mm.Map<ReadedHireProjectionContract>(It.IsAny<HireProjection>())).Returns(readedHireProjection);

            var actualResult = service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedHireProjection, actualResult);
            mockRepositoryHireProjection.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedHireProjectionContract>(It.IsAny<HireProjection>()), Times.Once);
        }
    }
}