using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Employee;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.Employee;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class EmployeeServiceTest : BaseDomainTest
    {
        private readonly EmployeeService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Employee>> _mockRepositoryEmployee;                
        private readonly Mock<ILog<EmployeeService>> _mockLogEmployeeService;
        private readonly Mock<UpdateEmployeeContractValidator> _mockUpdateEmployeeContractValidator;
        private readonly Mock<CreateEmployeeContractValidator> _mockCreateEmployeeContractValidator;
        private readonly Mock<IRepository<Consultant>> _mockRepositoryConsultant;
        private readonly Mock<IRepository<Role>> _mockRepositoryRole;

        public EmployeeServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryEmployee = new Mock<IRepository<Employee>>();            
            _mockLogEmployeeService = new Mock<ILog<EmployeeService>>();
            _mockUpdateEmployeeContractValidator = new Mock<UpdateEmployeeContractValidator>();
            _mockCreateEmployeeContractValidator = new Mock<CreateEmployeeContractValidator>();
            _mockRepositoryConsultant = new Mock<IRepository<Consultant>>();
            _mockRepositoryRole = new Mock<IRepository<Role>>();

            _service = new EmployeeService(
                _mockMapper.Object,
                _mockRepositoryEmployee.Object,
                MockUnitOfWork.Object,
                _mockLogEmployeeService.Object,
                _mockUpdateEmployeeContractValidator.Object,
                _mockCreateEmployeeContractValidator.Object,
                _mockRepositoryConsultant.Object,
                _mockRepositoryRole.Object
            );
        }

        [Fact(DisplayName = "Verify that create EmployeeService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateEmployeeService()
        {
            var contract = new CreateEmployeeContract();
            var expectedEmployee = new CreatedEmployeeContract();
            var consultants = new List<Consultant> { new Consultant { Id = 0 } }.AsQueryable();
            var roles = new List<Role> { new Role { Id = 0 } }.AsQueryable();
            _mockCreateEmployeeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>())).Returns(new Employee());
            _mockRepositoryEmployee.Setup(repoEmp => repoEmp.Create(It.IsAny<Employee>())).Returns(new Employee());
            _mockRepositoryConsultant.Setup(repoCon => repoCon.Query()).Returns(consultants);
            _mockRepositoryRole.Setup(repoRole => repoRole.Query()).Returns(roles);
            _mockMapper.Setup(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>())).Returns(expectedEmployee);

            var createdEmployee = _service.Create(contract);

            Assert.NotNull(createdEmployee);
            Assert.Equal(expectedEmployee, createdEmployee);
            _mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateEmployeeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>()), Times.Once);
            _mockRepositoryEmployee.Verify(mrt => mrt.Create(It.IsAny<Employee>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateEmployeeContract();
            var expectedEmployee = new CreatedEmployeeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateEmployeeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>())).Returns(expectedEmployee);

            var exception = Assert.Throws<Model.Exceptions.Employee.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateEmployeeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>()), Times.Never);
            _mockRepositoryEmployee.Verify(mrt => mrt.Create(It.IsAny<Employee>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete EmployeeService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteEmployeeService()
        {
            var Employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();
            _mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(Employees);

            _service.Delete(1);

            _mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryEmployee.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryEmployee.Verify(mrt => mrt.Delete(It.IsAny<Employee>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteEmployeeNotFoundException()
        {
            var expectedErrorMEssage = $"Employee not found for the ConsultantId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Employee.DeleteEmployeeNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryEmployee.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryEmployee.Verify(mrt => mrt.Delete(It.IsAny<Employee>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update EmployeeService when data is valid")]
        public void GivenUpdate_WhenDataIsValid_UpdateCorrectly()
        {
            var contract = new UpdateEmployeeContract();
            var consultants = new List<Consultant> { new Consultant { Id = 0 } }.AsQueryable();
            var roles = new List<Role> { new Role { Id = 0 } }.AsQueryable();
            _mockUpdateEmployeeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>())).Returns(new Employee());
            _mockRepositoryConsultant.Setup(repoCon => repoCon.Query()).Returns(consultants);
            _mockRepositoryRole.Setup(repoRole => repoRole.Query()).Returns(roles);

            _service.UpdateEmployee(contract);

            _mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateEmployeeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>()), Times.Once);
            _mockRepositoryEmployee.Verify(mrt => mrt.Update(It.IsAny<Employee>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateEmployeeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateEmployeeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>())).Returns(new Employee());

            var exception = Assert.Throws<Model.Exceptions.Employee.CreateContractInvalidException>(() => _service.UpdateEmployee(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateEmployeeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>()), Times.Never);
            _mockRepositoryEmployee.Verify(mrt => mrt.Update(It.IsAny<Employee>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();
            var readedEmployeeList = new List<ReadedEmployeeContract> { new ReadedEmployeeContract { Id = 1 } };
            _mockRepositoryEmployee.Setup(mrt => mrt.QueryEager()).Returns(Employees);
            _mockMapper.Setup(mm => mm.Map<List<ReadedEmployeeContract>>(It.IsAny<List<Employee>>())).Returns(readedEmployeeList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryEmployee.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedEmployeeContract>>(It.IsAny<List<Employee>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetById returns a value")]
        public void GivenGetById_WhenRegularCall_ReturnsValue()
        {
            var Employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();            
            _mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(Employees);
            _mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = _service.getById(1);

            Assert.NotNull(actualResult);
            _mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }


        [Fact(DisplayName = "Verify that GetByDNI returns a value")]
        public void GivenGetByDNI_WhenRegularCall_ReturnsValue()
        {
            var Employees = new List<Employee>() { new Employee() }.AsQueryable();            
            _mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(Employees);
            _mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = _service.getByDNI(1);

            Assert.NotNull(actualResult);
            _mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetByEmail returns a value")]
        public void GivenGetByEmail_WhenRegularCall_ReturnsValue()
        {
            var Employees = new List<Employee>() { new Employee() }.AsQueryable();
            _mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(Employees);
            _mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = _service.GetByEmail("email");

            Assert.NotNull(actualResult);
            _mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }
    }
}