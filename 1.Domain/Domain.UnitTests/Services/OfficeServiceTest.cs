using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Office;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.Office;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class OfficeServiceTest : BaseDomainTest
    {
        private readonly OfficeService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Office>> mockRepositoryOffice;        
        private readonly Mock<IRepository<Room>> mockRepositoryModelRoom;
        private readonly Mock<ILog<OfficeService>> mockLogOfficeService;
        private readonly Mock<UpdateOfficeContractValidator> mockUpdateOfficeContractValidator;
        private readonly Mock<CreateOfficeContractValidator> mockCreateOfficeContractValidator;

        public OfficeServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryOffice = new Mock<IRepository<Office>>();
            mockRepositoryModelRoom = new Mock<IRepository<Room>>();            
            mockLogOfficeService = new Mock<ILog<OfficeService>>();
            mockUpdateOfficeContractValidator = new Mock<UpdateOfficeContractValidator>();
            mockCreateOfficeContractValidator = new Mock<CreateOfficeContractValidator>();
            service = new OfficeService(
                mockMapper.Object,
                mockRepositoryOffice.Object,
                mockRepositoryModelRoom.Object,
                MockUnitOfWork.Object,
                mockLogOfficeService.Object,
                mockUpdateOfficeContractValidator.Object,
                mockCreateOfficeContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create OfficeService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateOfficeService()
        {
            var contract = new CreateOfficeContract();
            var expectedOffice = new CreatedOfficeContract();
            mockCreateOfficeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>())).Returns(new Office());
            mockRepositoryOffice.Setup(repoCom => repoCom.Create(It.IsAny<Office>())).Returns(new Office());
            mockMapper.Setup(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>())).Returns(expectedOffice);

            var createdOffice = service.Create(contract);

            Assert.NotNull(createdOffice);
            Assert.Equal(expectedOffice, createdOffice);
            mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateOfficeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>()), Times.Once);
            mockRepositoryOffice.Verify(mrt => mrt.Create(It.IsAny<Office>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateOfficeContract();
            var expectedOffice = new CreatedOfficeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateOfficeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>())).Returns(expectedOffice);

            var exception = Assert.Throws<Model.Exceptions.Office.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateOfficeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>()), Times.Never);
            mockRepositoryOffice.Verify(mrt => mrt.Create(It.IsAny<Office>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete OfficeService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteOfficeService()
        {
            var Offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            mockRepositoryOffice.Setup(mrt => mrt.Query()).Returns(Offices);

            service.Delete(1);

            mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryOffice.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryOffice.Verify(mrt => mrt.Delete(It.IsAny<Office>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteOfficeNotFoundException()
        {
            var expectedErrorMEssage = $"Office not found for the Office Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.Office.DeleteOfficeNotFoundException>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryOffice.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryOffice.Verify(mrt => mrt.Delete(It.IsAny<Office>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update OfficeService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateOfficeContract();
            mockUpdateOfficeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>())).Returns(new Office());

            service.Update(contract);

            mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateOfficeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>()), Times.Once);
            mockRepositoryOffice.Verify(mrt => mrt.Update(It.IsAny<Office>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateOfficeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateOfficeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>())).Returns(new Office());

            var exception = Assert.Throws<Model.Exceptions.Office.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateOfficeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>()), Times.Never);
            mockRepositoryOffice.Verify(mrt => mrt.Update(It.IsAny<Office>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            var readedOfficeList = new List<ReadedOfficeContract> { new ReadedOfficeContract { Id = 1 } };
            mockRepositoryOffice.Setup(mrt => mrt.QueryEager()).Returns(Offices);
            mockMapper.Setup(mm => mm.Map<List<ReadedOfficeContract>>(It.IsAny<List<Office>>())).Returns(readedOfficeList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryOffice.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedOfficeContract>>(It.IsAny<List<Office>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            var readedOffice = new ReadedOfficeContract { Id = 1 };
            mockRepositoryOffice.Setup(mrt => mrt.QueryEager()).Returns(Offices);
            mockMapper.Setup(mm => mm.Map<ReadedOfficeContract>(It.IsAny<Office>())).Returns(readedOffice);

            var actualResult = service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedOffice, actualResult);
            mockRepositoryOffice.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedOfficeContract>(It.IsAny<Office>()), Times.Once);
        }
    }
}