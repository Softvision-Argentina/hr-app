using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.CandidateProfile;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.CandidateProfile;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class CandidateProfileServiceTest : BaseDomainTest    
    {
        private readonly CandidateProfileService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<CandidateProfile>> mockRepositoryCandidateProfile;
        private readonly Mock<IRepository<Model.Community>> mockRepositoryModelCommunity;
        private readonly Mock<ILog<CandidateProfileService>> mockLogCandidateProfileService;
        private readonly Mock<UpdateCandidateProfileContractValidator> mockUpdateCandidateProfileContractValidator;
        private readonly Mock<CreateCandidateProfileContractValidator> mockCreateCandidateProfileContractValidator;

        public CandidateProfileServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();
            mockRepositoryModelCommunity = new Mock<IRepository<Model.Community>>();
            mockLogCandidateProfileService = new Mock<ILog<CandidateProfileService>>();
            mockUpdateCandidateProfileContractValidator = new Mock<UpdateCandidateProfileContractValidator>();
            mockCreateCandidateProfileContractValidator = new Mock<CreateCandidateProfileContractValidator>();
            service = new CandidateProfileService(
                mockMapper.Object, 
                mockRepositoryCandidateProfile.Object, 
                mockRepositoryModelCommunity.Object, MockUnitOfWork.Object, 
                mockLogCandidateProfileService.Object,
                mockUpdateCandidateProfileContractValidator.Object,
                mockCreateCandidateProfileContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create CandidateProfileService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCandidateProfileService()
        {
            var contract = new CreateCandidateProfileContract();
            var expectedCandidateProfile = new CreatedCandidateProfileContract();
            mockCreateCandidateProfileContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>())).Returns(expectedCandidateProfile);

            var createdCandidateProfile = service.Create(contract);

            Assert.NotNull(createdCandidateProfile);
            Assert.Equal(expectedCandidateProfile, createdCandidateProfile);
            mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateCandidateProfileContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<CreateCandidateProfileContract>()), Times.Once);
            mockRepositoryCandidateProfile.Verify(mrt => mrt.Create(It.IsAny<CandidateProfile>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCandidateProfileContract();
            var expectedCandidateProfile = new CreatedCandidateProfileContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateCandidateProfileContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>())).Returns(expectedCandidateProfile);

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateCandidateProfileContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCandidateProfileContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<CreateCandidateProfileContract>()), Times.Never);
            mockRepositoryCandidateProfile.Verify(mrt => mrt.Create(It.IsAny<CandidateProfile>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedCandidateProfileContract>(It.IsAny<CandidateProfile>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete CandidateProfileService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCandidateProfileService()
        {            
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile() { Id = 1 } }.AsQueryable();
            mockRepositoryCandidateProfile.Setup(mrt => mrt.Query()).Returns(candidateProfiles);

            service.Delete(1);

            mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryCandidateProfile.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryCandidateProfile.Verify(mrt => mrt.Delete(It.IsAny<CandidateProfile>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCandidateProfileNotFoundException()
        {            
            var expectedErrorMEssage = $"Profile not found for the Profile Id: {0}";

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.DeleteCandidateProfileNotFoundException>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryCandidateProfile.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryCandidateProfile.Verify(mrt => mrt.Delete(It.IsAny<CandidateProfile>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CandidateProfileService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCandidateProfileContract();
            mockUpdateCandidateProfileContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>())).Returns(new CandidateProfile());

            service.Update(contract);
            
            mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateCandidateProfileContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>()), Times.Once);
            mockRepositoryCandidateProfile.Verify(mrt => mrt.Update(It.IsAny<CandidateProfile>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCandidateProfileContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateCandidateProfileContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>())).Returns(new CandidateProfile());

            var exception = Assert.Throws<Model.Exceptions.CandidateProfile.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogCandidateProfileService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateCandidateProfileContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCandidateProfileContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<CandidateProfile>(It.IsAny<UpdateCandidateProfileContract>()), Times.Never);
            mockRepositoryCandidateProfile.Verify(mrt => mrt.Update(It.IsAny<CandidateProfile>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var candidateProfiles = new List<CandidateProfile>() { new CandidateProfile() { Id = 1 } }.AsQueryable();
            var readedCandidatePCList = new List<ReadedCandidateProfileContract> { new ReadedCandidateProfileContract { Id = 1 } };
            mockRepositoryCandidateProfile.Setup(mrt => mrt.QueryEager()).Returns(candidateProfiles);
            mockMapper.Setup(mm => mm.Map<List<ReadedCandidateProfileContract>>(It.IsAny<List<CandidateProfile>>())).Returns(readedCandidatePCList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryCandidateProfile.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedCandidateProfileContract>>(It.IsAny<List<CandidateProfile>>()), Times.Once);
        }
    }
}