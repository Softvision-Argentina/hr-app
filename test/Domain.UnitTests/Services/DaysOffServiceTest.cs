using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Model.Exceptions.DaysOff;
using Domain.Services.Contracts.DaysOff;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.DaysOff;
using Domain.Services.Interfaces.Services;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class DaysOffServiceTest : BaseDomainTest
    {
        private readonly DaysOffService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<DaysOff>> _mockRepositoryDaysOff;        
        private readonly Mock<ILog<DaysOffService>> _mockLogDaysOffService;
        private readonly Mock<IGoogleCalendarService> _mockCalendarservice;        
        private readonly Mock<UpdateDaysOffContractValidator> _mockUpdateDaysOffContractValidator;
        private readonly Mock<CreateDaysOffContractValidator> _mockCreateDaysOffContractValidator;

        public DaysOffServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryDaysOff = new Mock<IRepository<DaysOff>>();
            _mockCalendarservice = new Mock<IGoogleCalendarService>();
            _mockLogDaysOffService = new Mock<ILog<DaysOffService>>();
            _mockUpdateDaysOffContractValidator = new Mock<UpdateDaysOffContractValidator>();
            _mockCreateDaysOffContractValidator = new Mock<CreateDaysOffContractValidator>();
            _service = new DaysOffService(
                _mockMapper.Object,
                _mockRepositoryDaysOff.Object,
                MockUnitOfWork.Object,
                _mockLogDaysOffService.Object,
                _mockCalendarservice.Object,                
                _mockUpdateDaysOffContractValidator.Object,
                _mockCreateDaysOffContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create DaysOffService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateDaysOffService()
        {
            var contract = new CreateDaysOffContract();
            var expectedDaysOff = new CreatedDaysOffContract();
            var daysOff = new DaysOff
            {
                Status = DaysOffStatus.Accepted,
                Employee = new Employee { EmailAddress = "testc@mail" }
            };
            _mockCreateDaysOffContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDaysOffContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<DaysOff>(It.IsAny<CreateDaysOffContract>())).Returns(daysOff);
            _mockRepositoryDaysOff.Setup(repoCom => repoCom.Create(It.IsAny<DaysOff>())).Returns(new DaysOff());
            _mockMapper.Setup(mm => mm.Map<CreatedDaysOffContract>(It.IsAny<DaysOff>())).Returns(expectedDaysOff);

            var createdDaysOff = _service.Create(contract);

            Assert.NotNull(createdDaysOff);
            Assert.Equal(expectedDaysOff, createdDaysOff);            
            _mockCreateDaysOffContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDaysOffContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<DaysOff>(It.IsAny<CreateDaysOffContract>()), Times.Once);
            _mockRepositoryDaysOff.Verify(mrt => mrt.Create(It.IsAny<DaysOff>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedDaysOffContract>(It.IsAny<DaysOff>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateDaysOffContract();
            var expectedDaysOff = new CreatedDaysOffContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateDaysOffContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDaysOffContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedDaysOffContract>(It.IsAny<DaysOff>())).Returns(expectedDaysOff);

            var exception = Assert.Throws<Model.Exceptions.DaysOff.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);            
            _mockCreateDaysOffContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateDaysOffContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<DaysOff>(It.IsAny<CreateDaysOffContract>()), Times.Never);
            _mockRepositoryDaysOff.Verify(mrt => mrt.Create(It.IsAny<DaysOff>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedDaysOffContract>(It.IsAny<DaysOff>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete DaysOffService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteDaysOffService()
        {
            var daysOffList = new List<DaysOff>() { new DaysOff() { Id = 1 } }.AsQueryable();
            _mockRepositoryDaysOff.Setup(mrt => mrt.Query()).Returns(daysOffList);

            _service.Delete(1);
            
            _mockRepositoryDaysOff.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryDaysOff.Verify(mrt => mrt.Delete(It.IsAny<DaysOff>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteDaysOffNotFoundException()
        {
            var expectedErrorMEssage = $"Days off not found for the DaysOffId: {0}";

            var exception = Assert.Throws<Model.Exceptions.DaysOff.DeleteDaysOffNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockRepositoryDaysOff.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryDaysOff.Verify(mrt => mrt.Delete(It.IsAny<DaysOff>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update DaysOffService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateDaysOffContract();
            var daysOff = new DaysOff {
                Status = DaysOffStatus.Accepted,
                Employee = new Employee { EmailAddress = "test@mail.com" }
            };
            _mockRepositoryDaysOff.Setup(x => x.Update(It.IsAny<DaysOff>())).Returns(new DaysOff());
            _mockUpdateDaysOffContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDaysOffContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<DaysOff>(It.IsAny<UpdateDaysOffContract>())).Returns(daysOff);

            _service.Update(contract);
            
            _mockUpdateDaysOffContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDaysOffContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<DaysOff>(It.IsAny<UpdateDaysOffContract>()), Times.Once);
            _mockRepositoryDaysOff.Verify(mrt => mrt.Update(It.IsAny<DaysOff>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateDaysOffContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateDaysOffContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDaysOffContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<DaysOff>(It.IsAny<UpdateDaysOffContract>())).Returns(new DaysOff());

            var exception = Assert.Throws<CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);            
            _mockUpdateDaysOffContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateDaysOffContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<DaysOff>(It.IsAny<UpdateDaysOffContract>()), Times.Never);
            _mockRepositoryDaysOff.Verify(mrt => mrt.Update(It.IsAny<DaysOff>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var daysOffList = new List<DaysOff>() { new DaysOff() { Id = 1 } }.AsQueryable();
            var readedDaysOffList = new List<ReadedDaysOffContract> { new ReadedDaysOffContract { Id = 1 } };
            _mockRepositoryDaysOff.Setup(mrt => mrt.QueryEager()).Returns(daysOffList);
            _mockMapper.Setup(mm => mm.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>())).Returns(readedDaysOffList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryDaysOff.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<DaysOff>() { new DaysOff() { Id = 1 } }.AsQueryable();
            var readedDaysOff = new ReadedDaysOffContract { Id = 1 };
            _mockRepositoryDaysOff.Setup(mrt => mrt.QueryEager()).Returns(communities);
            _mockMapper.Setup(mm => mm.Map<ReadedDaysOffContract>(It.IsAny<DaysOff>())).Returns(readedDaysOff);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);           
            _mockRepositoryDaysOff.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedDaysOffContract>(It.IsAny<DaysOff>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that AcceptPetition runs correctly")]
        public void GivenAcceptPetition_WhenValidData_ReturnsValue()
        {
            var daysOffList = new List<DaysOff>() {
                new DaysOff() { 
                    Id = 1,
                    Employee = new Employee{ EmailAddress = "test@mail.com" } }
            }.AsQueryable();
            var readedDaysOff = new ReadedDaysOffContract { Id = 1 };
            _mockRepositoryDaysOff.Setup(x => x.Query()).Returns(daysOffList);
            _mockRepositoryDaysOff.Setup(x => x.Update(It.IsAny<DaysOff>())).Returns(new DaysOff());           

            _service.AcceptPetition(1);
           
            _mockRepositoryDaysOff.Verify(_ => _.Query(), Times.Once);
            _mockRepositoryDaysOff.Verify(_ => _.Update(It.IsAny<DaysOff>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that AcceptPetition throws exception")]
        public void GivenAcceptPetition_WhenInvalidData_ThrowsUpdateDaysOffNotFoundException()
        {            
            var exception = Assert.Throws<UpdateDaysOffNotFoundException>(() => _service.AcceptPetition(1));

            Assert.NotNull(exception);
        }

        [Fact(DisplayName = "Verify that ReadByDni returns a value")]
        public void GivenReadByDni_WhenRegularCall_ReturnsValue()
        {
            var daysOffList = new List<DaysOff>() {
                new DaysOff() { Id = 1, Employee = new Employee { DNI = 1 } } }
            .AsQueryable();
            var readedDaysOffList = new List<ReadedDaysOffContract> { new ReadedDaysOffContract { Id = 1 }};
            _mockRepositoryDaysOff.Setup(mrt => mrt.QueryEager()).Returns(daysOffList);
            _mockMapper.Setup(mm => mm.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>())).Returns(readedDaysOffList);

            var actualResult = _service.ReadByDni(1);

            Assert.NotNull(actualResult);
            _mockRepositoryDaysOff.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedDaysOffContract>>(It.IsAny<List<DaysOff>>()), Times.Once);
        }
    }
}