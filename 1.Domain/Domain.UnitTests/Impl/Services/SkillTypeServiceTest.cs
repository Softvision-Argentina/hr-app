using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.SkillType;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.SkillType;
using Domain.Services.Tests;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Impl.Services
{
    public class SkillTypeServiceTest : BaseDomainTest
    {
        private readonly SkillTypeService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<SkillType>> mockRepository;
        private readonly Mock<ILog<SkillTypeService>> mockLog;
        private readonly Mock<UpdateSkillTypeContractValidator> mockUpdateValidator;
        private readonly Mock<CreateSkillTypeContractValidator> mockCreateValidator;

        public SkillTypeServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepository = new Mock<IRepository<SkillType>>();
            mockLog = new Mock<ILog<SkillTypeService>>();
            mockUpdateValidator = new Mock<UpdateSkillTypeContractValidator>();
            mockCreateValidator = new Mock<CreateSkillTypeContractValidator>();
            service = new SkillTypeService(mockMapper.Object, mockRepository.Object, MockUnitOfWork.Object, mockLog.Object, mockUpdateValidator.Object, mockCreateValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CreatedSkillTypeContract when data is valid")]
        public void Should_CreatedSkillTypeContract_When_DataIsValid()
        {
            var contract = new CreateSkillTypeContract()
            {
                Name = "testName"
            };
            var expectedResult = new CreatedSkillTypeContract();
            mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>())).Returns(expectedResult);

            var result = service.Create(contract);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Once);
            mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Once);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
            mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillTypeContract()
            {
                Name = "testName"
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>()), Times.Once);
            mockRepository.Verify(_ => _.Query(), Times.Never);
            mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Never);
            mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillTypeException) when data for creation is invalid")]
        public void Should_ThrowInvalidSkillTypeException_When_CreationDataIsInvalid()
        {
            const string testName = "testName";
            var contract = new CreateSkillTypeContract()
            {
                Name = testName
            };
            mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult());
            mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.InvalidSkillTypeException>(() => service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal("The SkillType already exists .", result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>()), Times.Once);
            mockRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Never);
            mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete when data is valid")]
        public void Should_Delete_When_DataIsValid()
        {
            int id = 0;
            mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = id } }.AsQueryable());

            service.Delete(id);

            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepository.Verify(_ => _.Query(), Times.Once);
            mockRepository.Verify(_ => _.Delete(It.IsAny<SkillType>()), Times.Once);
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
            mockRepository.Verify(_ => _.Query(), Times.Once);
            mockRepository.Verify(_ => _.Delete(It.IsAny<SkillType>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update when data is valid")]
        public void Should_Update_When_DataIsValid()
        {
            int id = 0;
            var contract = new UpdateSkillTypeContract()
            {
                Name = "testName"
            };
            mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult());

            service.Update(contract);

            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            mockRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Once);
            mockRepository.Verify(_ => _.Update(It.IsAny<SkillType>()), Times.Once);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for update is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillTypeContract()
            {
                Name = "testName"
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            mockRepository.Verify(_ => _.Query(), Times.Never);
            mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Never);
            mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillTypeException) when data for update is invalid")]
        public void Should_ThrowInvalidSkillTypeException_When_UpdateDataIsInvalid()
        {
            const string testName = "testName";
            var contract = new UpdateSkillTypeContract()
            {
                Name = testName
            };
            mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult());
            mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.InvalidSkillTypeException>(() => service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal("The SkillType already exists .", result.Message);
            mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            mockRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Never);
            mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list when data is valid")]
        public void Should_List_When_DataIsValid()
        {
            var contract = new CreateSkillTypeContract()
            {
                Name = "testName"
            };
            var expectedResult = new List<ReadedSkillTypeContract>();
            mockMapper.Setup(_ => _.Map<List<ReadedSkillTypeContract>>(It.IsAny<List<SkillType>>())).Returns(expectedResult);

            var result = service.List();

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            mockRepository.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedSkillTypeContract>>(It.IsAny<List<SkillType>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read when data is valid")]
        public void Should_Read_When_DataIsValid()
        {
            int id = 0;
            var expectedResult = new ReadedSkillTypeContract();
            mockRepository.Setup(_ => _.QueryEager()).Returns(new List<SkillType>() { new SkillType() { Id = id } }.AsQueryable());
            mockMapper.Setup(_ => _.Map<ReadedSkillTypeContract>(It.IsAny<SkillType>())).Returns(expectedResult);

            var result = service.Read(id);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            mockRepository.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedSkillTypeContract>(It.IsAny<SkillType>()), Times.Once);
        }
    }
}
