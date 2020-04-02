using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Employee;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.Employee;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class EmployeeServiceTest : BaseDomainTest
    {
        private readonly EmployeeService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Employee>> mockRepositoryEmployee;        
        private readonly Mock<IRepository<Model.Employee>> mockRepositoryModelEmployee;
        private readonly Mock<ILog<EmployeeService>> mockLogEmployeeService;
        private readonly Mock<UpdateEmployeeContractValidator> mockUpdateEmployeeContractValidator;
        private readonly Mock<CreateEmployeeContractValidator> mockCreateEmployeeContractValidator;
        private readonly Mock<IRepository<Consultant>> mockRepositoryConsultant;
        private readonly Mock<IRepository<Role>> mockRepositoryRole;

        public EmployeeServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryEmployee = new Mock<IRepository<Employee>>();            
            mockRepositoryModelEmployee = new Mock<IRepository<Model.Employee>>();
            mockLogEmployeeService = new Mock<ILog<EmployeeService>>();
            mockUpdateEmployeeContractValidator = new Mock<UpdateEmployeeContractValidator>();
            mockCreateEmployeeContractValidator = new Mock<CreateEmployeeContractValidator>();
            mockRepositoryConsultant = new Mock<IRepository<Consultant>>();
            mockRepositoryRole = new Mock<IRepository<Role>>();

            service = new EmployeeService(
                mockMapper.Object,
                mockRepositoryEmployee.Object,
                MockUnitOfWork.Object,
                mockLogEmployeeService.Object,
                mockUpdateEmployeeContractValidator.Object,
                mockCreateEmployeeContractValidator.Object,
                mockRepositoryConsultant.Object,
                mockRepositoryRole.Object
            );
        }

        [Fact(DisplayName = "Verify that create EmployeeService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateEmployeeService()
        {
            var contract = new CreateEmployeeContract();
            var expectedEmployee = new CreatedEmployeeContract();
            var consultants = new List<Consultant> { new Consultant { Id = 0 } }.AsQueryable();
            var roles = new List<Role> { new Role { Id = 0 } }.AsQueryable();
            mockCreateEmployeeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>())).Returns(new Employee());
            mockRepositoryEmployee.Setup(repoEmp => repoEmp.Create(It.IsAny<Employee>())).Returns(new Employee());
            mockRepositoryConsultant.Setup(repoCon => repoCon.Query()).Returns(consultants);
            mockRepositoryRole.Setup(repoRole => repoRole.Query()).Returns(roles);
            mockMapper.Setup(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>())).Returns(expectedEmployee);

            var createdEmployee = service.Create(contract);

            Assert.NotNull(createdEmployee);
            Assert.Equal(expectedEmployee, createdEmployee);
            mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateEmployeeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>()), Times.Once);
            mockRepositoryEmployee.Verify(mrt => mrt.Create(It.IsAny<Employee>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateEmployeeContract();
            var expectedEmployee = new CreatedEmployeeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateEmployeeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>())).Returns(expectedEmployee);

            var exception = Assert.Throws<Model.Exceptions.Employee.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateEmployeeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>()), Times.Never);
            mockRepositoryEmployee.Verify(mrt => mrt.Create(It.IsAny<Employee>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete EmployeeService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteEmployeeService()
        {
            var Employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();
            mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(Employees);

            service.Delete(1);

            mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryEmployee.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryEmployee.Verify(mrt => mrt.Delete(It.IsAny<Employee>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteEmployeeNotFoundException()
        {
            var expectedErrorMEssage = $"Employee not found for the ConsultantId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Employee.DeleteEmployeeNotFoundException>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryEmployee.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryEmployee.Verify(mrt => mrt.Delete(It.IsAny<Employee>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update EmployeeService when data is valid")]
        public void GivenUpdate_WhenDataIsValid_UpdateCorrectly()
        {
            var contract = new UpdateEmployeeContract();
            var consultants = new List<Consultant> { new Consultant { Id = 0 } }.AsQueryable();
            var roles = new List<Role> { new Role { Id = 0 } }.AsQueryable();
            mockUpdateEmployeeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>())).Returns(new Employee());
            mockRepositoryConsultant.Setup(repoCon => repoCon.Query()).Returns(consultants);
            mockRepositoryRole.Setup(repoRole => repoRole.Query()).Returns(roles);

            service.UpdateEmployee(contract);

            mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateEmployeeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>()), Times.Once);
            mockRepositoryEmployee.Verify(mrt => mrt.Update(It.IsAny<Employee>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateEmployeeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateEmployeeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>())).Returns(new Employee());

            var exception = Assert.Throws<Model.Exceptions.Employee.CreateContractInvalidException>(() => service.UpdateEmployee(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateEmployeeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>()), Times.Never);
            mockRepositoryEmployee.Verify(mrt => mrt.Update(It.IsAny<Employee>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();
            var readedEmployeeList = new List<ReadedEmployeeContract> { new ReadedEmployeeContract { Id = 1 } };
            mockRepositoryEmployee.Setup(mrt => mrt.QueryEager()).Returns(Employees);
            mockMapper.Setup(mm => mm.Map<List<ReadedEmployeeContract>>(It.IsAny<List<Employee>>())).Returns(readedEmployeeList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryEmployee.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedEmployeeContract>>(It.IsAny<List<Employee>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetById returns a value")]
        public void GivenGetById_WhenRegularCall_ReturnsValue()
        {
            var Employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();            
            mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(Employees);
            mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = service.getById(1);

            Assert.NotNull(actualResult);
            mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }


        [Fact(DisplayName = "Verify that GetByDNI returns a value")]
        public void GivenGetByDNI_WhenRegularCall_ReturnsValue()
        {
            var Employees = new List<Employee>() { new Employee() }.AsQueryable();            
            mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(Employees);
            mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = service.getByDNI(1);

            Assert.NotNull(actualResult);
            mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetByEmail returns a value")]
        public void GivenGetByEmail_WhenRegularCall_ReturnsValue()
        {
            var Employees = new List<Employee>() { new Employee() }.AsQueryable();
            mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(Employees);
            mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = service.GetByEmail("email");

            Assert.NotNull(actualResult);
            mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }
    }
}