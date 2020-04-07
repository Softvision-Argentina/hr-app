using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Office;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.Office;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class OfficeServiceTest : BaseDomainTest
    {
        private readonly OfficeService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Office>> _mockRepositoryOffice;        
        private readonly Mock<IRepository<Room>> _mockRepositoryModelRoom;
        private readonly Mock<ILog<OfficeService>> _mockLogOfficeService;
        private readonly Mock<UpdateOfficeContractValidator> _mockUpdateOfficeContractValidator;
        private readonly Mock<CreateOfficeContractValidator> _mockCreateOfficeContractValidator;

        public OfficeServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryOffice = new Mock<IRepository<Office>>();
            _mockRepositoryModelRoom = new Mock<IRepository<Room>>();            
            _mockLogOfficeService = new Mock<ILog<OfficeService>>();
            _mockUpdateOfficeContractValidator = new Mock<UpdateOfficeContractValidator>();
            _mockCreateOfficeContractValidator = new Mock<CreateOfficeContractValidator>();
            _service = new OfficeService(
                _mockMapper.Object,
                _mockRepositoryOffice.Object,
                _mockRepositoryModelRoom.Object,
                MockUnitOfWork.Object,
                _mockLogOfficeService.Object,
                _mockUpdateOfficeContractValidator.Object,
                _mockCreateOfficeContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create OfficeService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateOfficeService()
        {
            var contract = new CreateOfficeContract();
            var expectedOffice = new CreatedOfficeContract();
            _mockCreateOfficeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>())).Returns(new Office());
            _mockRepositoryOffice.Setup(repoCom => repoCom.Create(It.IsAny<Office>())).Returns(new Office());
            _mockMapper.Setup(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>())).Returns(expectedOffice);

            var createdOffice = _service.Create(contract);

            Assert.NotNull(createdOffice);
            Assert.Equal(expectedOffice, createdOffice);
            _mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateOfficeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>()), Times.Once);
            _mockRepositoryOffice.Verify(mrt => mrt.Create(It.IsAny<Office>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateOfficeContract();
            var expectedOffice = new CreatedOfficeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateOfficeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>())).Returns(expectedOffice);

            var exception = Assert.Throws<Model.Exceptions.Office.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateOfficeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateOfficeContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<CreateOfficeContract>()), Times.Never);
            _mockRepositoryOffice.Verify(mrt => mrt.Create(It.IsAny<Office>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedOfficeContract>(It.IsAny<Office>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete OfficeService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteOfficeService()
        {
            var Offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            _mockRepositoryOffice.Setup(mrt => mrt.Query()).Returns(Offices);

            _service.Delete(1);

            _mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryOffice.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryOffice.Verify(mrt => mrt.Delete(It.IsAny<Office>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteOfficeNotFoundException()
        {
            var expectedErrorMEssage = $"Office not found for the Office Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.Office.DeleteOfficeNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryOffice.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryOffice.Verify(mrt => mrt.Delete(It.IsAny<Office>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update OfficeService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateOfficeContract();
            _mockUpdateOfficeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>())).Returns(new Office());

            _service.Update(contract);

            _mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateOfficeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>()), Times.Once);
            _mockRepositoryOffice.Verify(mrt => mrt.Update(It.IsAny<Office>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateOfficeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateOfficeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>())).Returns(new Office());

            var exception = Assert.Throws<Model.Exceptions.Office.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogOfficeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateOfficeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateOfficeContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Office>(It.IsAny<UpdateOfficeContract>()), Times.Never);
            _mockRepositoryOffice.Verify(mrt => mrt.Update(It.IsAny<Office>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            var readedOfficeList = new List<ReadedOfficeContract> { new ReadedOfficeContract { Id = 1 } };
            _mockRepositoryOffice.Setup(mrt => mrt.QueryEager()).Returns(Offices);
            _mockMapper.Setup(mm => mm.Map<List<ReadedOfficeContract>>(It.IsAny<List<Office>>())).Returns(readedOfficeList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryOffice.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedOfficeContract>>(It.IsAny<List<Office>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Offices = new List<Office>() { new Office() { Id = 1 } }.AsQueryable();
            var readedOffice = new ReadedOfficeContract { Id = 1 };
            _mockRepositoryOffice.Setup(mrt => mrt.QueryEager()).Returns(Offices);
            _mockMapper.Setup(mm => mm.Map<ReadedOfficeContract>(It.IsAny<Office>())).Returns(readedOffice);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal(readedOffice, actualResult);
            _mockRepositoryOffice.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedOfficeContract>(It.IsAny<Office>()), Times.Once);
        }
    }
}