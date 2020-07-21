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
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class RoleServiceTest : BaseDomainTest
    {
        private readonly RoleService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Role>> _mockRepositoryRole;        
        private readonly Mock<ILog<SkillTypeService>> _mockLogRoleService;
        private readonly Mock<UpdateRoleContractValidator> _mockUpdateRoleContractValidator;
        private readonly Mock<CreateRoleContractValidator> _mockCreateRoleContractValidator;

        public RoleServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryRole = new Mock<IRepository<Role>>();            
            _mockLogRoleService = new Mock<ILog<SkillTypeService>>();
            _mockUpdateRoleContractValidator = new Mock<UpdateRoleContractValidator>();
            _mockCreateRoleContractValidator = new Mock<CreateRoleContractValidator>();
            _service = new RoleService(
                _mockMapper.Object,
                _mockRepositoryRole.Object,
                MockUnitOfWork.Object,                
                _mockLogRoleService.Object,
                _mockUpdateRoleContractValidator.Object,
                _mockCreateRoleContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create RoleService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateRoleService()
        {
            var contract = new CreateRoleContract();
            var expectedRole = new CreatedRoleContract();
            _mockCreateRoleContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoleContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Role>(It.IsAny<CreateRoleContract>())).Returns(new Role());
            _mockRepositoryRole.Setup(repoCom => repoCom.Create(It.IsAny<Role>())).Returns(new Role());
            _mockMapper.Setup(mm => mm.Map<CreatedRoleContract>(It.IsAny<Role>())).Returns(expectedRole);

            var createdRole = _service.Create(contract);

            Assert.NotNull(createdRole);
            Assert.Equal(expectedRole, createdRole);
            _mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateRoleContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoleContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Role>(It.IsAny<CreateRoleContract>()), Times.Once);
            _mockRepositoryRole.Verify(mrt => mrt.Create(It.IsAny<Role>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedRoleContract>(It.IsAny<Role>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateRoleContract();
            var expectedRole = new CreatedRoleContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateRoleContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoleContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedRoleContract>(It.IsAny<Role>())).Returns(expectedRole);

            var exception = Assert.Throws<Model.Exceptions.Role.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateRoleContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateRoleContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Role>(It.IsAny<CreateRoleContract>()), Times.Never);
            _mockRepositoryRole.Verify(mrt => mrt.Create(It.IsAny<Role>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedRoleContract>(It.IsAny<Role>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete RoleService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteRoleService()
        {
            var Roles = new List<Role>() { new Role() { Id = 1 } }.AsQueryable();
            _mockRepositoryRole.Setup(mrt => mrt.Query()).Returns(Roles);

            _service.Delete(1);

            _mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryRole.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryRole.Verify(mrt => mrt.Delete(It.IsAny<Role>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteRoleNotFoundException()
        {
            var expectedErrorMEssage = $"Role not found for the Role Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.Role.DeleteRoleNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryRole.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryRole.Verify(mrt => mrt.Delete(It.IsAny<Role>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update RoleService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateRoleContract();
            _mockUpdateRoleContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoleContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Role>(It.IsAny<UpdateRoleContract>())).Returns(new Role());

            _service.Update(contract);

            _mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateRoleContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoleContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Role>(It.IsAny<UpdateRoleContract>()), Times.Once);
            _mockRepositoryRole.Verify(mrt => mrt.Update(It.IsAny<Role>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateRoleContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateRoleContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoleContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Role>(It.IsAny<UpdateRoleContract>())).Returns(new Role());

            var exception = Assert.Throws<Model.Exceptions.Role.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogRoleService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateRoleContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateRoleContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Role>(It.IsAny<UpdateRoleContract>()), Times.Never);
            _mockRepositoryRole.Verify(mrt => mrt.Update(It.IsAny<Role>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Roles = new List<Role>() { new Role() { Id = 1 } }.AsQueryable();
            var readedRoleList = new List<ReadedRoleContract> { new ReadedRoleContract { Id = 1 } };
            _mockRepositoryRole.Setup(mrt => mrt.QueryEager()).Returns(Roles);
            _mockMapper.Setup(mm => mm.Map<List<ReadedRoleContract>>(It.IsAny<List<Role>>())).Returns(readedRoleList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryRole.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedRoleContract>>(It.IsAny<List<Role>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Roles = new List<Role>() { new Role() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedRole = new ReadedRoleContract { Id = 1, Name = "Name" };
            _mockRepositoryRole.Setup(mrt => mrt.QueryEager()).Returns(Roles);
            _mockMapper.Setup(mm => mm.Map<ReadedRoleContract>(It.IsAny<Role>())).Returns(readedRole);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            _mockRepositoryRole.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedRoleContract>(It.IsAny<Role>()), Times.Once);
        }
    }
}