using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Skill;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.Skill;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class SkillServiceTest : BaseDomainTest
    {
        private readonly SkillService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<SkillType>> _mockRepositorySkillType;
        private readonly Mock<IRepository<Skill>> _mockRepositorySkill;
        private readonly Mock<ILog<SkillService>> _mockLog;
        private readonly Mock<UpdateSkillContractValidator> _mockUpdateValidator;
        private readonly Mock<CreateSkillContractValidator> _mockCreateValidator;

        public SkillServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositorySkill = new Mock<IRepository<Skill>>();
            _mockRepositorySkillType = new Mock<IRepository<SkillType>>();
            _mockLog = new Mock<ILog<SkillService>>();
            _mockUpdateValidator = new Mock<UpdateSkillContractValidator>();
            _mockCreateValidator = new Mock<CreateSkillContractValidator>();
            _service = new SkillService(_mockMapper.Object, _mockRepositorySkill.Object, _mockRepositorySkillType.Object, MockUnitOfWork.Object, _mockLog.Object, _mockUpdateValidator.Object, _mockCreateValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CreatedSkillContract when data is valid")]
        public void Should_CreatedSkillContract_When_DataIsValid()
        {
            var contract = new CreateSkillContract();
            var expectedResult = new CreatedSkillContract();
            _mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult());
            _mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() }.AsQueryable());
            _mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>())).Returns(new Skill());
            _mockMapper.Setup(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>())).Returns(expectedResult);

            var result = _service.Create(contract);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Once);
            _mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Once);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
            _mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Never);
            _mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Never);
            _mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            _mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            _mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillException) when data for creation is invalid")]
        public void Should_ThrowInvalidSkillException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillContract();
            var expectedError = "The skill already exists .";
            _mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult());
            _mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() { Id = 1 } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.Skill.InvalidSkillException>(() => _service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedError, result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Never);
            _mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            _mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            _mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete when data is valid")]
        public void Should_Delete_When_DataIsValid()
        {
            int id = 0;
            _mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() }.AsQueryable());

            _service.Delete(id);

            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Delete(It.IsAny<Skill>()), Times.Once);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws DeleteSkillNotFoundException when data is invalid")]
        public void Should_ThrowDeleteSkillNotFoundException_When_DeleteDataIsInvalid()
        {
            int id = 0;
            var expectedMessage = $"Skill not found for the skillId: {id}";

            var result = Assert.Throws<Model.Exceptions.Skill.DeleteSkillNotFoundException>(() => _service.Delete(id));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Delete(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update when data is valid")]
        public void Should_Update_When_DataIsValid()
        {
            var contract = new UpdateSkillContract()
            {
                Name = "testName"
            };
            _mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            _mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() }.AsQueryable());

            _service.Update(contract);

            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Once);
            _mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Once);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for update is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillContract()
            {
                Name = "testName"
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Never);
            _mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Never);
            _mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            _mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillException) when data for update is invalid")]
        public void Should_ThrowInvalidSkillException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillContract();
            const string expectedMessage = "The skill already exists .";
            _mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            _mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() { Id = 1 } }.AsQueryable());
            //mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            //mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.Skill.InvalidSkillException>(() => _service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Never);
            _mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            _mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (SkillTypeNotFoundException) when data for update is invalid")]
        public void Should_ThrowSkillTypeNotFoundException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillContract();
            const string expectedMessage = "The skill type 0 was not found.";
            _mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            _mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1 } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.SkillTypeNotFoundException>(() => _service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Once);
            _mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            _mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list when data is valid")]
        public void Should_List_When_DataIsValid()
        {
            var expectedResult = new List<ReadedSkillContract>();
            _mockMapper.Setup(_ => _.Map<List<ReadedSkillContract>>(It.IsAny<List<Skill>>())).Returns(expectedResult);

            var result = _service.List();

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            _mockRepositorySkill.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedSkillContract>>(It.IsAny<List<Skill>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read when data is valid")]
        public void Should_Read_When_DataIsValid()
        {
            int id = 0;
            var expectedResult = new ReadedSkillContract();
            _mockRepositorySkill.Setup(_ => _.QueryEager()).Returns(new List<Skill>() { new Skill() }.AsQueryable());
            _mockMapper.Setup(_ => _.Map<ReadedSkillContract>(It.IsAny<Skill>())).Returns(expectedResult);

            var result = _service.Read(id);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            _mockRepositorySkill.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedSkillContract>(It.IsAny<Skill>()), Times.Once);
        }
    }
}
