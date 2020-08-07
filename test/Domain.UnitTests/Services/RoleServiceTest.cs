// <copyright file="RoleServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.Role;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.Role;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class RoleServiceTest : BaseDomainTest
    {
        private readonly RoleService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Role>> mockRepositoryRole;
        private readonly Mock<ILog<SkillTypeService>> mockLogRoleService;
        private readonly Mock<UpdateRoleContractValidator> mockUpdateRoleContractValidator;
        private readonly Mock<CreateRoleContractValidator> mockCreateRoleContractValidator;

        public RoleServiceTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositoryRole = new Mock<IRepository<Role>>();
            this.mockLogRoleService = new Mock<ILog<SkillTypeService>>();
            this.mockUpdateRoleContractValidator = new Mock<UpdateRoleContractValidator>();
            this.mockCreateRoleContractValidator = new Mock<CreateRoleContractValidator>();
            this.service = new RoleService(
                this.mockMapper.Object,
                this.mockRepositoryRole.Object,
                this.MockUnitOfWork.Object,
                this.mockLogRoleService.Object,
                this.mockUpdateRoleContractValidator.Object,
                this.mockCreateRoleContractValidator.Object);
        }

        [Fact(DisplayName = "Verify that create RoleService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateRoleService()
        {
            var contract = new CreateRoleContract();
            var expectedRole = new CreatedRoleContract();
            this.mockCreateRoleContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoleContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Role>(It.IsAny<CreateRoleContract>())).Returns(new Role());
            this.mockRepositoryRole.Setup(repoCom => repoCom.Create(It.IsAny<Role>())).Returns(new Role());
            this.mockMapper.Setup(mm => mm.Map<CreatedRoleContract>(It.IsAny<Role>())).Returns(expectedRole);

            var createdRole = this.service.Create(contract);

            Assert.NotNull(createdRole);
            Assert.Equal(expectedRole, createdRole);
            this.mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateRoleContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoleContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Role>(It.IsAny<CreateRoleContract>()), Times.Once);
            this.mockRepositoryRole.Verify(mrt => mrt.Create(It.IsAny<Role>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<CreatedRoleContract>(It.IsAny<Role>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateRoleContract();
            var expectedRole = new CreatedRoleContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateRoleContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoleContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<CreatedRoleContract>(It.IsAny<Role>())).Returns(expectedRole);

            var exception = Assert.Throws<Model.Exceptions.Role.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateRoleContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoleContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Role>(It.IsAny<CreateRoleContract>()), Times.Never);
            this.mockRepositoryRole.Verify(mrt => mrt.Create(It.IsAny<Role>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            this.mockMapper.Verify(mm => mm.Map<CreatedRoleContract>(It.IsAny<Role>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete RoleService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteRoleService()
        {
            var roles = new List<Role>() { new Role() { Id = 1 } }.AsQueryable();
            this.mockRepositoryRole.Setup(mrt => mrt.Query()).Returns(roles);

            this.service.Delete(1);

            this.mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositoryRole.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryRole.Verify(mrt => mrt.Delete(It.IsAny<Role>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteRoleNotFoundException()
        {
            var expectedErrorMEssage = $"Role not found for the Role Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.Role.DeleteRoleNotFoundException>(() => this.service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            this.mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepositoryRole.Verify(mrt => mrt.Query(), Times.Once);
            this.mockRepositoryRole.Verify(mrt => mrt.Delete(It.IsAny<Role>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update RoleService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateRoleContract();
            this.mockUpdateRoleContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoleContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(mm => mm.Map<Role>(It.IsAny<UpdateRoleContract>())).Returns(new Role());

            this.service.Update(contract);

            this.mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateRoleContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoleContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Role>(It.IsAny<UpdateRoleContract>()), Times.Once);
            this.mockRepositoryRole.Verify(mrt => mrt.Update(It.IsAny<Role>()), Times.Once);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateRoleContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateRoleContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoleContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            this.mockMapper.Setup(mm => mm.Map<Role>(It.IsAny<UpdateRoleContract>())).Returns(new Role());

            var exception = Assert.Throws<Model.Exceptions.Role.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            this.mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateRoleContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoleContract>>()), Times.Once);
            this.mockMapper.Verify(mm => mm.Map<Role>(It.IsAny<UpdateRoleContract>()), Times.Never);
            this.mockRepositoryRole.Verify(mrt => mrt.Update(It.IsAny<Role>()), Times.Never);
            this.MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var roles = new List<Role>() { new Role() { Id = 1 } }.AsQueryable();
            var readedRoleList = new List<ReadedRoleContract> { new ReadedRoleContract { Id = 1 } };
            this.mockRepositoryRole.Setup(mrt => mrt.QueryEager()).Returns(roles);
            this.mockMapper.Setup(mm => mm.Map<List<ReadedRoleContract>>(It.IsAny<List<Role>>())).Returns(readedRoleList);

            var actualResult = this.service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            this.mockRepositoryRole.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedRoleContract>>(It.IsAny<List<Role>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var roles = new List<Role>() { new Role() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedRole = new ReadedRoleContract { Id = 1, Name = "Name" };
            this.mockRepositoryRole.Setup(mrt => mrt.QueryEager()).Returns(roles);
            this.mockMapper.Setup(mm => mm.Map<ReadedRoleContract>(It.IsAny<Role>())).Returns(readedRole);

            var actualResult = this.service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            this.mockRepositoryRole.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedRoleContract>(It.IsAny<Role>()), Times.Once);
        }
    }
}