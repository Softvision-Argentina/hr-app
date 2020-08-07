// <copyright file="SkillServiceTest.cs" company="Softvision">
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
    using Domain.Services.Contracts.Skill;
    using Domain.Services.Impl.Services;
    using Domain.Services.Impl.UnitTests.Dummy;
    using Domain.Services.Impl.Validators.Skill;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

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
            this.mockMapper = new Mock<IMapper>();
            this.mockRepositorySkill = new Mock<IRepository<Skill>>();
            this.mockRepositorySkillType = new Mock<IRepository<SkillType>>();
            this.mockLog = new Mock<ILog<SkillService>>();
            this.mockUpdateValidator = new Mock<UpdateSkillContractValidator>();
            this.mockCreateValidator = new Mock<CreateSkillContractValidator>();
            this.service = new SkillService(this.mockMapper.Object, this.mockRepositorySkill.Object, this.mockRepositorySkillType.Object, this.MockUnitOfWork.Object, this.mockLog.Object, this.mockUpdateValidator.Object, this.mockCreateValidator.Object);
        }

        [Fact(DisplayName = "Verify that create CreatedSkillContract when data is valid")]
        public void Should_CreatedSkillContract_When_DataIsValid()
        {
            var contract = new CreateSkillContract();
            var expectedResult = new CreatedSkillContract();
            this.mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult());
            this.mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() }.AsQueryable());
            this.mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>())).Returns(new Skill());
            this.mockMapper.Setup(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>())).Returns(expectedResult);

            var result = this.service.Create(contract);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            this.mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Once);
            this.mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Once);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for creation is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => this.service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Never);
            this.mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Never);
            this.mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            this.mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            this.mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillException) when data for creation is invalid")]
        public void Should_ThrowInvalidSkillException_When_CreationDataIsInvalid()
        {
            var contract = new CreateSkillContract();
            var expectedError = "The skill already exists .";
            this.mockCreateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>())).Returns(new ValidationResult());
            this.mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() { Id = 1 } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.Skill.InvalidSkillException>(() => this.service.Create(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedError, result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockCreateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<CreateSkillContract>>()), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<CreateSkillContract>()), Times.Never);
            this.mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            this.mockRepositorySkill.Verify(_ => _.Create(It.IsAny<Skill>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
            this.mockMapper.Verify(_ => _.Map<CreatedSkillContract>(It.IsAny<Skill>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete when data is valid")]
        public void Should_Delete_When_DataIsValid()
        {
            int id = 0;
            this.mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() }.AsQueryable());

            this.service.Delete(id);

            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Delete(It.IsAny<Skill>()), Times.Once);
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
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Delete(It.IsAny<Skill>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update when data is valid")]
        public void Should_Update_When_DataIsValid()
        {
            var contract = new UpdateSkillContract()
            {
                Name = "testName",
            };
            this.mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            this.mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() }.AsQueryable());

            this.service.Update(contract);

            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            this.mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Once);
            this.mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Once);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error (CreateContractInvalidException) when data for update is invalid")]
        public void Should_ThrowCreateContractInvalidException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillContract()
            {
                Name = "testName",
            };
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            this.mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));

            var result = Assert.Throws<Model.Exceptions.Skill.CreateContractInvalidException>(() => this.service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(validationFailure.ErrorMessage, result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Never);
            this.mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Never);
            this.mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            this.mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (InvalidSkillException) when data for update is invalid")]
        public void Should_ThrowInvalidSkillException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillContract();
            const string expectedMessage = "The skill already exists .";
            this.mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            this.mockRepositorySkill.Setup(_ => _.Query()).Returns(new List<Skill>() { new Skill() { Id = 1 } }.AsQueryable());

            // mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            // mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1, Name = testName } }.AsQueryable());
            var result = Assert.Throws<Model.Exceptions.Skill.InvalidSkillException>(() => this.service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Once);
            this.mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Never);
            this.mockRepositorySkillType.Verify(_ => _.Query(), Times.Never);
            this.mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that throws error (SkillTypeNotFoundException) when data for update is invalid")]
        public void Should_ThrowSkillTypeNotFoundException_When_UpdateDataIsInvalid()
        {
            var contract = new UpdateSkillContract();
            const string expectedMessage = "The skill type 0 was not found.";
            this.mockUpdateValidator.Setup(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>())).Returns(new ValidationResult());
            this.mockMapper.Setup(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>())).Returns(new Skill());
            this.mockRepositorySkillType.Setup(_ => _.Query()).Returns(new List<SkillType>() { new SkillType() { Id = 1 } }.AsQueryable());

            var result = Assert.Throws<Model.Exceptions.SkillType.SkillTypeNotFoundException>(() => this.service.Update(contract));

            Assert.NotNull(result);
            Assert.Equal(expectedMessage, result.Message);
            this.mockLog.Verify(_ => _.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            this.mockUpdateValidator.Verify(_ => _.Validate(It.IsAny<ValidationContext<UpdateSkillContract>>()), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Query(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<Skill>(It.IsAny<UpdateSkillContract>()), Times.Once);
            this.mockRepositorySkillType.Verify(_ => _.Query(), Times.Once);
            this.mockRepositorySkill.Verify(_ => _.Update(It.IsAny<Skill>()), Times.Never);
            this.MockUnitOfWork.Verify(_ => _.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list when data is valid")]
        public void Should_List_When_DataIsValid()
        {
            var expectedResult = new List<ReadedSkillContract>();
            this.mockMapper.Setup(_ => _.Map<List<ReadedSkillContract>>(It.IsAny<List<Skill>>())).Returns(expectedResult);

            var result = this.service.List();

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            this.mockRepositorySkill.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<List<ReadedSkillContract>>(It.IsAny<List<Skill>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read when data is valid")]
        public void Should_Read_When_DataIsValid()
        {
            int id = 0;
            var expectedResult = new ReadedSkillContract();
            this.mockRepositorySkill.Setup(_ => _.QueryEager()).Returns(new List<Skill>() { new Skill() }.AsQueryable());
            this.mockMapper.Setup(_ => _.Map<ReadedSkillContract>(It.IsAny<Skill>())).Returns(expectedResult);

            var result = this.service.Read(id);

            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            this.mockRepositorySkill.Verify(_ => _.QueryEager(), Times.Once);
            this.mockMapper.Verify(_ => _.Map<ReadedSkillContract>(It.IsAny<Skill>()), Times.Once);
        }
    }
}
