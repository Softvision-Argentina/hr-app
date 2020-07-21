using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.SkillType;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.SkillType;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class SkillTypeServiceTest : BaseDomainTest
    {
        private readonly SkillTypeService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<SkillType>> _mockRepository;
        private readonly Mock<ILog<SkillTypeService>> _mockLog;
        private readonly Mock<UpdateSkillTypeContractValidator> _mockUpdateValidator;
        private readonly Mock<CreateSkillTypeContractValidator> _mockCreateValidator;

        public SkillTypeServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository<SkillType>>();
            _mockLog = new Mock<ILog<SkillTypeService>>();
            _mockUpdateValidator = new Mock<UpdateSkillTypeContractValidator>();
            _mockCreateValidator = new Mock<CreateSkillTypeContractValidator>();
            _service = new SkillTypeService(_mockMapper.Object, _mockRepository.Object, MockUnitOfWork.Object, _mockLog.Object, _mockUpdateValidator.Object, _mockCreateValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CreatedSkillTypeContract when data is valid")]
        public void Should_CreatedSkillTypeContract_When_DataIsValid()
        {
            var contract = new CreateSkillTypeContract()
            {
                Name = "testName"
            };
            var expectedResult = new CreatedSkillTypeContract();
            _mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>())).Returns(expectedResult);

            var result = _service.Create(contract);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockRepository.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Once);
            _mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Once);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
            _mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillTypeContract()
            {
                Name = "testName"
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>()), Times.Once);
            _mockRepository.Verify(_ => _.Query(), Times.Never);
            _mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Never);
            _mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            _mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillTypeException) when data for creation is invalid")]
        public void Should_ThrowInvalidSkillTypeException_When_CreationDataIsInvalid()
        {
            const string testName = "testName";
            var contract = new CreateSkillTypeContract()
            {
                Name = testName
            };
            _mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult());
            _mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.InvalidSkillTypeException>(() => _service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal("The SkillType already exists .", result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>()), Times.Once);
            _mockRepository.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Never);
            _mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            _mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete when data is valid")]
        public void Should_Delete_When_DataIsValid()
        {
            int id = 0;
            _mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = id } }.AsQueryable());

            _service.Delete(id);

            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepository.Verify(_ => _.Query(), Times.Once);
            _mockRepository.Verify(_ => _.Delete(It.IsAny<SkillType>()), Times.Once);
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
            _mockRepository.Verify(_ => _.Query(), Times.Once);
            _mockRepository.Verify(_ => _.Delete(It.IsAny<SkillType>()), Times.Never);
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
            _mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult());

            _service.Update(contract);

            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            _mockRepository.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Once);
            _mockRepository.Verify(_ => _.Update(It.IsAny<SkillType>()), Times.Once);
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
            _mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            _mockRepository.Verify(_ => _.Query(), Times.Never);
            _mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Never);
            _mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
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
            _mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult());
            _mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.InvalidSkillTypeException>(() => _service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal("The SkillType already exists .", result.Message);
            _mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            _mockRepository.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Never);
            _mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
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
            _mockMapper.Setup(_ => _.Map<List<ReadedSkillTypeContract>>(It.IsAny<List<SkillType>>())).Returns(expectedResult);

            var result = _service.List();

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedSkillTypeContract>>(It.IsAny<List<SkillType>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read when data is valid")]
        public void Should_Read_When_DataIsValid()
        {
            int id = 0;
            var expectedResult = new ReadedSkillTypeContract();
            _mockRepository.Setup(_ => _.QueryEager()).Returns(new List<SkillType>() { new SkillType() { Id = id } }.AsQueryable());
            _mockMapper.Setup(_ => _.Map<ReadedSkillTypeContract>(It.IsAny<SkillType>())).Returns(expectedResult);

            var result = _service.Read(id);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            _mockRepository.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedSkillTypeContract>(It.IsAny<SkillType>()), Times.Once);
        }
    }
}
