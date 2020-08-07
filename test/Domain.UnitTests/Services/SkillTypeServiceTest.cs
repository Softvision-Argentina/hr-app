// <copyright file="SkillTypeServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.SkillType;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.SkillType;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

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
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository = new Mock<IRepository<SkillType>>();
            this.mockLog = new Mock<ILog<SkillTypeService>>();
            this.mockUpdateValidator = new Mock<UpdateSkillTypeContractValidator>();
            this.mockCreateValidator = new Mock<CreateSkillTypeContractValidator>();
            this.service = new SkillTypeService(this.mockMapper.Object, this.mockRepository.Object, this.MockUnitOfWork.Object, this.mockLog.Object, this.mockUpdateValidator.Object, this.mockCreateValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CreatedSkillTypeContract when data is valid")]
        public void Should_CreatedSkillTypeContract_When_DataIsValid()
        {
            var contract = new CreateSkillTypeContract()
            {
                Name = "testName",
            };
            var expectedResult = new CreatedSkillTypeContract();
            this.mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>())).Returns(expectedResult);

            var result = this.service.Create(contract);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockRepository.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Once);
            this.mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Once);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillTypeContract()
            {
                Name = "testName",
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>()), Times.Once);
            this.mockRepository.Verify(_ => _.Query(), Times.Never);
            this.mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Never);
            this.mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            this.mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillTypeException) when data for creation is invalid")]
        public void Should_ThrowInvalidSkillTypeException_When_CreationDataIsInvalid()
        {
            const string testName = "testName";
            var contract = new CreateSkillTypeContract()
            {
                Name = testName,
            };
            this.mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>())).Returns(new ValidationResult());
            this.mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.InvalidSkillTypeException>(() => this.service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal("The SkillType already exists .", result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillTypeContract>>()), Times.Once);
            this.mockRepository.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<CreateSkillTypeContract>()), Times.Never);
            this.mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            this.mockMapper.Verify(_ => _.Map<CreatedSkillTypeContract>(It.IsAny<SkillType>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete when data is valid")]
        public void Should_Delete_When_DataIsValid()
        {
            int id = 0;
            this.mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = id } }.AsQueryable());

            this.service.Delete(id);

            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepository.Verify(_ => _.Query(), Times.Once);
            this.mockRepository.Verify(_ => _.Delete(It.IsAny<SkillType>()), Times.Once);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws DeleteSkillNotFoundException when data is invalid")]
        public void Should_ThrowDeleteSkillNotFoundException_When_DeleteDataIsInvalid()
        {
            int id = 0;
            var expectedMessage = $"Skill not found for the skillId: {id}";

            var result = Assert.Throws<Model.Exceptions.Skill.DeleteSkillNotFoundException>(() => this.service.Delete(id));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockRepository.Verify(_ => _.Query(), Times.Once);
            this.mockRepository.Verify(_ => _.Delete(It.IsAny<SkillType>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update when data is valid")]
        public void Should_Update_When_DataIsValid()
        {
            var contract = new UpdateSkillTypeContract()
            {
                Name = "testName",
            };
            this.mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult());

            this.service.Update(contract);

            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            this.mockRepository.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Once);
            this.mockRepository.Verify(_ => _.Update(It.IsAny<SkillType>()), Times.Once);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for update is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillTypeContract()
            {
                Name = "testName",
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            this.mockRepository.Verify(_ => _.Query(), Times.Never);
            this.mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Never);
            this.mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillTypeException) when data for update is invalid")]
        public void Should_ThrowInvalidSkillTypeException_When_UpdateDataIsInvalid()
        {
            const string testName = "testName";
            var contract = new UpdateSkillTypeContract()
            {
                Name = testName,
            };
            this.mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>())).Returns(new ValidationResult());
            this.mockRepository.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.InvalidSkillTypeException>(() => this.service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal("The SkillType already exists .", result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillTypeContract>>()), Times.Once);
            this.mockRepository.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<SkillType>(It.IsAny<UpdateSkillTypeContract>()), Times.Never);
            this.mockRepository.Verify(_ => _.Create(It.IsAny<SkillType>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list when data is valid")]
        public void Should_List_When_DataIsValid()
        {
            var contract = new CreateSkillTypeContract()
            {
                Name = "testName",
            };
            var expectedResult = new List<ReadedSkillTypeContract>();
            this.mockMapper.Setup(_ => _.Map<List<ReadedSkillTypeContract>>(It.IsAny<List<SkillType>>())).Returns(expectedResult);

            var result = this.service.List();

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            this.mockRepository.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedSkillTypeContract>>(It.IsAny<List<SkillType>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read when data is valid")]
        public void Should_Read_When_DataIsValid()
        {
            int id = 0;
            var expectedResult = new ReadedSkillTypeContract();
            this.mockRepository.Setup(_ => _.QueryEager()).Returns(new List<SkillType>() { new SkillType() { Id = id } }.AsQueryable());
            this.mockMapper.Setup(_ => _.Map<ReadedSkillTypeContract>(It.IsAny<SkillType>())).Returns(expectedResult);

            var result = this.service.Read(id);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            this.mockRepository.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedSkillTypeContract>(It.IsAny<SkillType>()), Times.Once);
        }
    }
}
