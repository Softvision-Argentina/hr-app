using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.CandidateProfile;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.CandidateProfile;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class CandidateProfileServiceTest : BaseDomainTest    
    {
        private readonly CandidateProfileService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<CandidateProfile>> _mockRepositoryCandidateProfile;
        private readonly Mock<IRepository<Community>> _mockRepositoryModelCommunity;
        private readonly Mock<ILog<CandidateProfileService>> _mockLogCandidateProfileService;
        private readonly Mock<UpdateCandidateProfileContractValidator> _mockUpdateCandidateProfileContractValidator;
        private readonly Mock<CreateCandidateProfileContractValidator> _mockCreateCandidateProfileContractValidator;

        public CandidateProfileServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();
            _mockRepositoryModelCommunity = new Mock<IRepository<Community>>();
            _mockLogCandidateProfileService = new Mock<ILog<CandidateProfileService>>();
            _mockUpdateCandidateProfileContractValidator = new Mock<UpdateCandidateProfileContractValidator>();
            _mockCreateCandidateProfileContractValidator = new Mock<CreateCandidateProfileContractValidator>();
            _service = new CandidateProfileService(
                _mockMapper.Object, 
                _mockRepositoryCandidateProfile.Object, 
                _mockRepositoryModelCommunity.Object, 
                MockUnitOfWork.Object, 
                _mockLogCandidateProfileService.Object,
                _mockUpdateCandidateProfileContractValidator.Object,
                _mockCreateCandidateProfileContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create CandidateProfileService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCandidateProfileService()
        {
            var contract = new CreateCandidateProfileContract();
            var expectedCandidateProfile = new CreatedCandidateProfileContract();
            _mockCreateCandidateProfileContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>())).Returns(expectedCandidateProfile);

            var createdCandidateProfile = _service.Create(contract);

            Assert.NotNull(createdCandidateProfile);
            Assert.Equal(expectedCandidateProfile, createdCandidateProfile);
            _mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateCandidateProfileContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<CreateCandidateProfileContract>()), Times.Once);
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Create(It.IsAny<CandidateProfile>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCandidateProfileContract();
            var expectedCandidateProfile = new CreatedCandidateProfileContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateCandidateProfileContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>())).Returns(expectedCandidateProfile);

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateCandidateProfileContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<CreateCandidateProfileContract>()), Times.Never);
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Create(It.IsAny<CandidateProfile>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that given a model that already exists on the database, throws an exception")]
        public void GivenCreate_WhenModelExists_ThrowsInvalidCandidateProfileException()
        {
            var contract = new CreateCandidateProfileContract { Name ="Name" };
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile { Name = "Name", Id = 1 } }.AsQueryable();
            _mockCreateCandidateProfileContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>())).Returns(new ValidationResult());
            _mockRepositoryCandidateProfile.Setup(mockRep => mockRep.Query()).Returns(candidateProfiles);

            Exception ex = Assert.Throws<InvalidCandidateProfileException>(() => _service.Create(contract));

            _mockCreateCandidateProfileContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>()), Times.Once);
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Query(), Times.Once);
            Assert.NotNull(ex);
            Assert.IsType<InvalidCandidateProfileException>(ex);            
            Assert.Equal($"The Profile already exists .", ex.Message);
        }

        [Fact(DisplayName = "Verify that delete CandidateProfileService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCandidateProfileService()
        {            
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile() { Id = 1 } }.AsQueryable();
            _mockRepositoryCandidateProfile.Setup(mrt => mrt.Query()).Returns(candidateProfiles);

            _service.Delete(1);

            _mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Delete(It.IsAny<CandidateProfile>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCandidateProfileNotFoundException()
        {            
            var expectedErrorMEssage = $"Profile not found for the Profile Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.DeleteCandidateProfileNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Delete(It.IsAny<CandidateProfile>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CandidateProfileService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCandidateProfileContract();
            _mockUpdateCandidateProfileContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>())).Returns(new CandidateProfile());

            _service.Update(contract);
            
            _mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateCandidateProfileContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>()), Times.Once);
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Update(It.IsAny<CandidateProfile>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCandidateProfileContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateCandidateProfileContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>())).Returns(new CandidateProfile());

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateCandidateProfileContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>()), Times.Never);
            _mockRepositoryCandidateProfile.Verify(mrt => mrt.Update(It.IsAny<CandidateProfile>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile() { Id = 1 } }.AsQueryable();
            var readedCandidatePCList = new List<ReadedCandidateProfileContract> { new ReadedCandidateProfileContract { Id = 1 } };
            _mockRepositoryCandidateProfile.Setup(mrt => mrt.QueryEager()).Returns(candidateProfiles);
            _mockMapper.Setup(mm => mm.Map<List<ReadedCandidateProfileContract>>(It.IsAny<List<CandidateProfile>>())).Returns(readedCandidatePCList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryCandidateProfile.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedCandidateProfileContract>>(It.IsAny<List<CandidateProfile>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile() { Id = 1, Name="Name" } }.AsQueryable();
            var readedCandidatePC = new ReadedCandidateProfileContract { Id = 1, Name ="Name" };
            _mockRepositoryCandidateProfile.Setup(mrt => mrt.QueryEager()).Returns(candidateProfiles);
            _mockMapper.Setup(mm => mm.Map<ReadedCandidateProfileContract>(It.IsAny<CandidateProfile>())).Returns(readedCandidatePC);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            _mockRepositoryCandidateProfile.Verify(_ => _.QueryEager(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedCandidateProfileContract>(It.IsAny<CandidateProfile>()), Times.Once);
        }
    }
}