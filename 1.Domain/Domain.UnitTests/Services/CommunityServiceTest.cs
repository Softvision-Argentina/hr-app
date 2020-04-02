using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Exceptions.Community;
using Domain.Services.Contracts.Community;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.Validators.Community;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class CommunityServiceTest : BaseDomainTest
    {
        private readonly CommunityService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<Community>> mockRepositoryCommunity;
        private readonly Mock<IRepository<CandidateProfile>> mockRepositoryCandidateProfile;
        private readonly Mock<IRepository<Model.Community>> mockRepositoryModelCommunity;        
        private readonly Mock<ILog<CommunityService>> mockLogCommunityService;
        private readonly Mock<UpdateCommunityContractValidator> mockUpdateCommunityContractValidator;
        private readonly Mock<CreateCommunityContractValidator> mockCreateCommunityContractValidator;

        public CommunityServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockRepositoryCommunity = new Mock<IRepository<Community>>();
            mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();
            mockRepositoryModelCommunity = new Mock<IRepository<Model.Community>>();            
            mockLogCommunityService = new Mock<ILog<CommunityService>>();
            mockUpdateCommunityContractValidator = new Mock<UpdateCommunityContractValidator>();
            mockCreateCommunityContractValidator = new Mock<CreateCommunityContractValidator>();
            service = new CommunityService(
                mockMapper.Object,
                mockRepositoryCommunity.Object,
                mockRepositoryCandidateProfile.Object,
                MockUnitOfWork.Object,
                mockLogCommunityService.Object,
                mockUpdateCommunityContractValidator.Object,
                mockCreateCommunityContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create CommunityService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCommunityService()
        {
            var contract = new CreateCommunityContract();
            var expectedCommunity = new CreatedCommunityContract();
            mockCreateCommunityContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>())).Returns(new Community());
            mockRepositoryCommunity.Setup(repoCom => repoCom.Create(It.IsAny<Community>())).Returns(new Community());
            mockMapper.Setup(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>())).Returns(expectedCommunity);

            var createdCommunity = service.Create(contract);

            Assert.NotNull(createdCommunity);
            Assert.Equal(expectedCommunity, createdCommunity);
            mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            mockCreateCommunityContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>()), Times.Once);
            mockRepositoryCommunity.Verify(mrt => mrt.Create(It.IsAny<Community>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            mockMapper.Verify(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCommunityContract();
            var expectedCommunity = new CreatedCommunityContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockCreateCommunityContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>())).Returns(expectedCommunity);

            var exception = Assert.Throws<Model.Exceptions.Community.CreateContractInvalidException>(() => service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockCreateCommunityContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>()), Times.Never);
            mockRepositoryCommunity.Verify(mrt => mrt.Create(It.IsAny<Community>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            mockMapper.Verify(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete CommunityService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCommunityService()
        {
            var Communitys = new List<Community>() { new Community() { Id = 1 } }.AsQueryable();
            mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(Communitys);

            service.Delete(1);

            mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            mockRepositoryCommunity.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryCommunity.Verify(mrt => mrt.Delete(It.IsAny<Community>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCommunityNotFoundException()
        {
            var expectedErrorMEssage = $"Community not found for the CommunityId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Community.DeleteCommunityNotFoundException>(() => service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockRepositoryCommunity.Verify(mrt => mrt.Query(), Times.Once);
            mockRepositoryCommunity.Verify(mrt => mrt.Delete(It.IsAny<Community>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CommunityService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCommunityContract();
            mockUpdateCommunityContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>())).Returns(new ValidationResult());
            mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>())).Returns(new Community());

            service.Update(contract);

            mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            mockUpdateCommunityContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>()), Times.Once);
            mockRepositoryCommunity.Verify(mrt => mrt.Update(It.IsAny<Community>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCommunityContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            mockUpdateCommunityContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>())).Returns(new Community());

            var exception = Assert.Throws<Model.Exceptions.Community.CreateContractInvalidException>(() => service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            mockUpdateCommunityContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>()), Times.Once);
            mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>()), Times.Never);
            mockRepositoryCommunity.Verify(mrt => mrt.Update(It.IsAny<Community>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var Communitys = new List<Community>() { new Community() { Id = 1 } }.AsQueryable();
            var readedCommunityList = new List<ReadedCommunityContract> { new ReadedCommunityContract { Id = 1 } };
            mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(Communitys);
            mockMapper.Setup(mm => mm.Map<List<ReadedCommunityContract>>(It.IsAny<List<Community>>())).Returns(readedCommunityList);

            var actualResult = service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            mockRepositoryCommunity.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedCommunityContract>>(It.IsAny<List<Community>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var Communitys = new List<Community>() { new Community() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCommunity = new ReadedCommunityContract { Id = 1, Name = "Name" };
            mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(Communitys);
            mockMapper.Setup(mm => mm.Map<ReadedCommunityContract>(It.IsAny<Community>())).Returns(readedCommunity);

            var actualResult = service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            mockRepositoryCommunity.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedCommunityContract>(It.IsAny<Community>()), Times.Once);
        }
    }
}