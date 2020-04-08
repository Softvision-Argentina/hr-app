using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.EmployeeCasualty;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.EmployeeCasualty;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class EmployeeCasualtyServiceTest : BaseDomainTest
    {
        private readonly EmployeeCasualtyService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<EmployeeCasualty>> _mockRepositoryEmployeeCasualty;        
        private readonly Mock<ILog<EmployeeCasualtyService>> _mockLogEmployeeCasualtyService;
        private readonly Mock<UpdateEmployeeCasualtyContractValidator> _mockUpdateEmployeeCasualtyContractValidator;
        private readonly Mock<CreateEmployeeCasualtyContractValidator> _mockCreateEmployeeCasualtyContractValidator;

        public EmployeeCasualtyServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryEmployeeCasualty = new Mock<IRepository<EmployeeCasualty>>();            
            _mockLogEmployeeCasualtyService = new Mock<ILog<EmployeeCasualtyService>>();
            _mockUpdateEmployeeCasualtyContractValidator = new Mock<UpdateEmployeeCasualtyContractValidator>();
            _mockCreateEmployeeCasualtyContractValidator = new Mock<CreateEmployeeCasualtyContractValidator>();
            _service = new EmployeeCasualtyService(
                _mockMapper.Object,
                _mockRepositoryEmployeeCasualty.Object,                
                MockUnitOfWork.Object,
                _mockLogEmployeeCasualtyService.Object,
                _mockUpdateEmployeeCasualtyContractValidator.Object,
                _mockCreateEmployeeCasualtyContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create EmployeeCasualtyService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateEmployeeCasualtyService()
        {
            var contract = new CreateEmployeeCasualtyContract();
            var expectedEmployeeCasualty = new CreatedEmployeeCasualtyContract();
            _mockCreateEmployeeCasualtyContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());
            _mockRepositoryEmployeeCasualty.Setup(repoCom => repoCom.Create(It.IsAny<EmployeeCasualty>())).Returns(new EmployeeCasualty());
            _mockMapper.Setup(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(expectedEmployeeCasualty);

            var createdEmployeeCasualty = _service.Create(contract);

            Assert.NotNull(createdEmployeeCasualty);
            Assert.Equal(expectedEmployeeCasualty, createdEmployeeCasualty);
            _mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateEmployeeCasualtyContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>()), Times.Once);
            _mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Create(It.IsAny<EmployeeCasualty>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateEmployeeCasualtyContract();
            var expectedEmployeeCasualty = new CreatedEmployeeCasualtyContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateEmployeeCasualtyContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(expectedEmployeeCasualty);

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateEmployeeCasualtyContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeCasualtyContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<CreateEmployeeCasualtyContract>()), Times.Never);
            _mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Create(It.IsAny<EmployeeCasualty>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete EmployeeCasualtyService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteEmployeeCasualtyService()
        {
            var EmployeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            _mockRepositoryEmployeeCasualty.Setup(mrt => mrt.Query()).Returns(EmployeeCasualtys);

            _service.Delete(1);

            _mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Delete(It.IsAny<EmployeeCasualty>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteEmployeeCasualtyNotFoundException()
        {
            var expectedErrorMEssage = $"Employee casualty not found for the EmployeeCasualtyId: {0}";

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.DeleteEmployeeCasualtyNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Delete(It.IsAny<EmployeeCasualty>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update EmployeeCasualtyService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateEmployeeCasualtyContract();
            _mockUpdateEmployeeCasualtyContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());

            _service.Update(contract);

            _mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateEmployeeCasualtyContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>()), Times.Once);
            _mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Update(It.IsAny<EmployeeCasualty>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateEmployeeCasualtyContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateEmployeeCasualtyContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>())).Returns(new EmployeeCasualty());

            var exception = Assert.Throws<Model.Exceptions.EmployeeCasualty.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogEmployeeCasualtyService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateEmployeeCasualtyContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeCasualtyContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<EmployeeCasualty>(It.IsAny<UpdateEmployeeCasualtyContract>()), Times.Never);
            _mockRepositoryEmployeeCasualty.Verify(mrt => mrt.Update(It.IsAny<EmployeeCasualty>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var EmployeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            var readedEmployeeCasualtyList = new List<ReadedEmployeeCasualtyContract> { new ReadedEmployeeCasualtyContract { Id = 1 } };
            _mockRepositoryEmployeeCasualty.Setup(mrt => mrt.QueryEager()).Returns(EmployeeCasualtys);
            _mockMapper.Setup(mm => mm.Map<List<ReadedEmployeeCasualtyContract>>(It.IsAny<List<EmployeeCasualty>>())).Returns(readedEmployeeCasualtyList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryEmployeeCasualty.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedEmployeeCasualtyContract>>(It.IsAny<List<EmployeeCasualty>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var EmployeeCasualtys = new List<EmployeeCasualty>() { new EmployeeCasualty() { Id = 1 } }.AsQueryable();
            var readedEmployeeCasualty = new ReadedEmployeeCasualtyContract { Id = 1 };
            _mockRepositoryEmployeeCasualty.Setup(mrt => mrt.QueryEager()).Returns(EmployeeCasualtys);
            _mockMapper.Setup(mm => mm.Map<ReadedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>())).Returns(readedEmployeeCasualty);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);            
            _mockRepositoryEmployeeCasualty.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedEmployeeCasualtyContract>(It.IsAny<EmployeeCasualty>()), Times.Once);
        }
    }
}