// <copyright file="EmployeeServiceTest.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
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
    using Xunit;

    public class EmployeeServiceTest : BaseDomainTest
    {
        private readonly EmployeeService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Employee>> mockRepositoryEmployee;
        private readonly Mock<ILog<EmployeeService>> mockLogEmployeeService;
        private readonly Mock<UpdateEmployeeContractValidator> mockUpdateEmployeeContractValidator;
        private readonly Mock<CreateEmployeeContractValidator> mockCreateEmployeeContractValidator;
        private readonly Mock<IRepository<User>> mockRepositoryUser;
        private readonly Mock<IRepository<Role>> mockRepositoryRole;

        public EmployeeServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryEmployee = new Mock<IRepository<Employee>>();
            this.mockLogEmployeeService = new Mock<ILog<EmployeeService>>();
            this.mockUpdateEmployeeContractValidator = new Mock<UpdateEmployeeContractValidator>();
            this.mockCreateEmployeeContractValidator = new Mock<CreateEmployeeContractValidator>();
            this.mockRepositoryUser = new Mock<IRepository<User>>();
            this.mockRepositoryRole = new Mock<IRepository<Role>>();

            this.service = new EmployeeService(
                this.mockMapper.Object,
                this.mockRepositoryEmployee.Object,
                this.MockUnitOfWork.Object,
                this.mockLogEmployeeService.Object,
                this.mockUpdateEmployeeContractValidator.Object,
                this.mockCreateEmployeeContractValidator.Object,
                this.mockRepositoryUser.Object,
                this.mockRepositoryRole.Object);
        }

        [Fact(DisplayName = "Verify that create EmployeeService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateEmployeeService()
        {
            var contract = new CreateEmployeeContract();
            var expectedEmployee = new CreatedEmployeeContract();
            var users = new List<User> { new User { Id = 0 } }.AsQueryable();
            var roles = new List<Role> { new Role { Id = 0 } }.AsQueryable();
            this.mockCreateEmployeeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>())).Returns(new Employee());
            this.mockRepositoryEmployee.Setup(repoEmp => repoEmp.Create(It.IsAny<Employee>())).Returns(new Employee());
            this.mockRepositoryUser.Setup(repoCon => repoCon.Query()).Returns(users);
            this.mockRepositoryRole.Setup(repoRole => repoRole.Query()).Returns(roles);
            this.mockMapper.Setup(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>())).Returns(expectedEmployee);

            var createdEmployee = this.service.Create(contract);

            Assert.NotNull(createdEmployee);
            Assert.Equal(expectedEmployee, createdEmployee);
            this.mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateEmployeeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>()), Times.Once);
            this.mockRepositoryEmployee.Verify(mrt => mrt.Create(It.IsAny<Employee>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateEmployeeContract();
            var expectedEmployee = new CreatedEmployeeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateEmployeeContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>())).Returns(expectedEmployee);

            var exception = Assert.Throws<Model.Exceptions.Employee.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateEmployeeContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateEmployeeContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<CreateEmployeeContract>()), Times.Never);
            this.mockRepositoryEmployee.Verify(mrt => mrt.Create(It.IsAny<Employee>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedEmployeeContract>(It.IsAny<Employee>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete EmployeeService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteEmployeeService()
        {
            var employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();
            this.mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(employees);

            this.service.Delete(1);

            this.mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryEmployee.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryEmployee.Verify(mrt => mrt.Delete(It.IsAny<Employee>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteEmployeeNotFoundException()
        {
            var expectedErrorMEssage = $"Employee not found for the UserId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Employee.DeleteEmployeeNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryEmployee.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryEmployee.Verify(mrt => mrt.Delete(It.IsAny<Employee>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update EmployeeService when data is valid")]
        public void GivenUpdate_WhenDataIsValid_UpdateCorrectly()
        {
            var contract = new UpdateEmployeeContract();
            var users = new List<User> { new User { Id = 0 } }.AsQueryable();
            var roles = new List<Role> { new Role { Id = 0 } }.AsQueryable();
            this.mockUpdateEmployeeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>())).Returns(new Employee());
            this.mockRepositoryUser.Setup(repoCon => repoCon.Query()).Returns(users);
            this.mockRepositoryRole.Setup(repoRole => repoRole.Query()).Returns(roles);

            this.service.UpdateEmployee(contract);

            this.mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateEmployeeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>()), Times.Once);
            this.mockRepositoryEmployee.Verify(mrt => mrt.Update(It.IsAny<Employee>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateEmployeeContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateEmployeeContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>())).Returns(new Employee());

            var exception = Assert.Throws<Model.Exceptions.Employee.CreateContractInvalidException>(() => this.service.UpdateEmployee(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogEmployeeService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateEmployeeContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateEmployeeContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Employee>(It.IsAny<UpdateEmployeeContract>()), Times.Never);
            this.mockRepositoryEmployee.Verify(mrt => mrt.Update(It.IsAny<Employee>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();
            var readedEmployeeList = new List<ReadedEmployeeContract> { new ReadedEmployeeContract { Id = 1 } };
            this.mockRepositoryEmployee.Setup(mrt => mrt.QueryEager()).Returns(employees);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedEmployeeContract>>(It.IsAny<List<Employee>>())).Returns(readedEmployeeList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryEmployee.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedEmployeeContract>>(It.IsAny<List<Employee>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetById returns a value")]
        public void GivenGetById_WhenRegularCall_ReturnsValue()
        {
            var employees = new List<Employee>() { new Employee() { Id = 1 } }.AsQueryable();
            this.mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(employees);
            this.mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = this.service.GetById(1);

            Assert.NotNull(actualResult);
            this.mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetByDNI returns a value")]
        public void GivenGetByDNI_WhenRegularCall_ReturnsValue()
        {
            var employees = new List<Employee>() { new Employee() }.AsQueryable();
            this.mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(employees);
            this.mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = this.service.GetByDNI(1);

            Assert.NotNull(actualResult);
            this.mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that GetByEmail returns a value")]
        public void GivenGetByEmail_WhenRegularCall_ReturnsValue()
        {
            var employees = new List<Employee>() { new Employee() }.AsQueryable();
            this.mockRepositoryEmployee.Setup(mrt => mrt.Query()).Returns(employees);
            this.mockMapper.Setup(mm => mm.Map<Employee>(It.IsAny<Employee>())).Returns(new Employee());

            var actualResult = this.service.GetByEmail("email");

            Assert.NotNull(actualResult);
            this.mockRepositoryEmployee.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<Employee>(It.IsAny<Employee>()), Times.Once);
        }
    }
}