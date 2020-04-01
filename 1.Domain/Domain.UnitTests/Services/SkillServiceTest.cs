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
        private readonly SkillService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<SkillType>> mockRepositorySkillType;
        private readonly Mock<IRepository<Skill>> mockRepositorySkill;
        private readonly Mock<ILog<SkillService>> mockLog;
        private readonly Mock<UpdateSkillContractValidator> mockUpdateValidator;
        private readonly Mock<CreateSkillContractValidator> mockCreateValidator;

        public SkillServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositorySkill = new Mock<IRepository<Skill>>();
            mockRepositorySkillType = new Mock<IRepository<SkillType>>();
            mockLog = new Mock<ILog<SkillService>>();
            mockUpdateValidator = new Mock<UpdateSkillContractValidator>();
            mockCreateValidator = new Mock<CreateSkillContractValidator>();
            service = new SkillService(mockMapper.Object, mockRepositorySkill.Object, mockRepositorySkillType.Object, MockUnitOfWork.Object, mockLog.Object, mockUpdateValidator.Object, mockCreateValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CreatedSkillContract when data is valid")]
        public void Should_CreatedSkillContract_When_DataIsValid()
        {
            var contract = new CreateSkillContract();
            var expectedResult = new CreatedSkillContract();
            mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult());
            mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() }.AsQueryable());
            mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>())).Returns(new Skill());
            mockMapper.Setup(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>())).Returns(expectedResult);

            var result = service.Create(contract);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Once);
            mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Once);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            mockRepositorySkill.Verify(_ => _.Query(), Times.Never);
            mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Never);
            mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillException) when data for creation is invalid")]
        public void Should_ThrowInvalidSkillException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillContract();
            var expectedError = "The skill already exists .";
            mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult());
            mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() { Id = 1 } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.Skill.InvalidSkillException>(() => service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedError, result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Never);
            mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete when data is valid")]
        public void Should_Delete_When_DataIsValid()
        {
            int id = 0;
            mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() }.AsQueryable());

            service.Delete(id);

            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            mockRepositorySkill.Verify(_ => _.Delete(It.IsAny<Skill>()), Times.Once);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws DeleteSkillNotFoundException when data is invalid")]
        public void Should_ThrowDeleteSkillNotFoundException_When_DeleteDataIsInvalid()
        {
            int id = 0;
            var expectedMessage = $"Skill not found for the skillId: {id}";

            var result = Assert.Throws<Model.Exceptions.Skill.DeleteSkillNotFoundException>(() => service.Delete(id));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            mockRepositorySkill.Verify(_ => _.Delete(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update when data is valid")]
        public void Should_Update_When_DataIsValid()
        {
            int id = 0;
            var contract = new UpdateSkillContract()
            {
                Name = "testName"
            };
            mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() }.AsQueryable());

            service.Update(contract);

            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Once);
            mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Once);
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
            mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            mockRepositorySkill.Verify(_ => _.Query(), Times.Never);
            mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Never);
            mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillException) when data for update is invalid")]
        public void Should_ThrowInvalidSkillException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillContract();
            const string expectedMessage = "The skill already exists .";
            mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() { Id = 1 } }.AsQueryable());
            //mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            //mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.Skill.InvalidSkillException>(() => service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Never);
            mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (SkillTypeNotFoundException) when data for update is invalid")]
        public void Should_ThrowSkillTypeNotFoundException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillContract();
            const string expectedMessage = "The skill type 0 was not found.";
            mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1 } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.SkillTypeNotFoundException>(() => service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Once);
            mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list when data is valid")]
        public void Should_List_When_DataIsValid()
        {
            var expectedResult = new List<ReadedSkillContract>();
            mockMapper.Setup(_ => _.Map<List<ReadedSkillContract>>(It.IsAny<List<Skill>>())).Returns(expectedResult);

            var result = service.List();

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            mockRepositorySkill.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedSkillContract>>(It.IsAny<List<Skill>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read when data is valid")]
        public void Should_Read_When_DataIsValid()
        {
            int id = 0;
            var expectedResult = new ReadedSkillContract();
            mockRepositorySkill.Setup(_ => _.QueryEager()).Returns(new List<Skill>() { new Skill() }.AsQueryable());
            mockMapper.Setup(_ => _.Map<ReadedSkillContract>(It.IsAny<Skill>())).Returns(expectedResult);

            var result = service.Read(id);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            mockRepositorySkill.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedSkillContract>(It.IsAny<Skill>()), Times.Once);
        }
    }
}