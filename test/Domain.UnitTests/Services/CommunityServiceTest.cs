using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.Community;
using Domain.Services.Impl.Services;
using Domain.Services.Impl.UnitTests.Dummy;
using Domain.Services.Impl.Validators.Community;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Impl.UnitTests.Services
{
    public class CommunityServiceTest : BaseDomainTest
    {
        private readonly CommunityService _service;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepository<Community>> _mockRepositoryCommunity;
        private readonly Mock<IRepository<CandidateProfile>> _mockRepositoryCandidateProfile;        
        private readonly Mock<ILog<CommunityService>> _mockLogCommunityService;
        private readonly Mock<UpdateCommunityContractValidator> _mockUpdateCommunityContractValidator;
        private readonly Mock<CreateCommunityContractValidator> _mockCreateCommunityContractValidator;

        public CommunityServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepositoryCommunity = new Mock<IRepository<Community>>();
            _mockRepositoryCandidateProfile = new Mock<IRepository<CandidateProfile>>();            
            _mockLogCommunityService = new Mock<ILog<CommunityService>>();
            _mockUpdateCommunityContractValidator = new Mock<UpdateCommunityContractValidator>();
            _mockCreateCommunityContractValidator = new Mock<CreateCommunityContractValidator>();
            _service = new CommunityService(
                _mockMapper.Object,
                _mockRepositoryCommunity.Object,
                _mockRepositoryCandidateProfile.Object,
                MockUnitOfWork.Object,
                _mockLogCommunityService.Object,
                _mockUpdateCommunityContractValidator.Object,
                _mockCreateCommunityContractValidator.Object
            );
        }

        [Fact(DisplayName = "Verify that create CommunityService when data is valid")]
        public void GivenCreate_WhenDataIsValid_CreateCommunityService()
        {
            var contract = new CreateCommunityContract();
            var expectedCommunity = new CreatedCommunityContract();
            _mockCreateCommunityContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>())).Returns(new Community());
            _mockRepositoryCommunity.Setup(repoCom => repoCom.Create(It.IsAny<Community>())).Returns(new Community());
            _mockMapper.Setup(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>())).Returns(expectedCommunity);

            var createdCommunity = _service.Create(contract);

            Assert.NotNull(createdCommunity);
            Assert.Equal(expectedCommunity, createdCommunity);
            _mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(4));
            _mockCreateCommunityContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>()), Times.Once);
            _mockRepositoryCommunity.Verify(mrt => mrt.Create(It.IsAny<Community>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
            _mockMapper.Verify(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that create throws error when data for creation is invalid")]
        public void GivenCreate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new CreateCommunityContract();
            var expectedCommunity = new CreatedCommunityContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockCreateCommunityContractValidator.Setup(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>())).Returns(expectedCommunity);

            var exception = Assert.Throws<Model.Exceptions.Community.CreateContractInvalidException>(() => _service.Create(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockCreateCommunityContractValidator.Verify(ctcv => ctcv.Validate(It.IsAny<ValidationContext<CreateCommunityContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<CreateCommunityContract>()), Times.Never);
            _mockRepositoryCommunity.Verify(mrt => mrt.Create(It.IsAny<Community>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
            _mockMapper.Verify(mm => mm.Map<CreatedCommunityContract>(It.IsAny<Community>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that delete CommunityService when data is valid")]
        public void GivenDelete_WhenDataIsValid_DeleteCommunityService()
        {
            var communities = new List<Community>() { new Community() { Id = 1 } }.AsQueryable();
            _mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(communities);

            _service.Delete(1);

            _mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(2));
            _mockRepositoryCommunity.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryCommunity.Verify(mrt => mrt.Delete(It.IsAny<Community>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that delete throws error when data for deletion is invalid")]
        public void GivenDelete__WhenDataIsInvalid_ThrowDeleteCommunityNotFoundException()
        {
            var expectedErrorMEssage = $"Community not found for the CommunityId: {0}";

            var exception = Assert.Throws<Model.Exceptions.Community.DeleteCommunityNotFoundException>(() => _service.Delete(0));

            Assert.NotNull(exception);
            Assert.Equal(expectedErrorMEssage, exception.Message);
            _mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockRepositoryCommunity.Verify(mrt => mrt.Query(), Times.Once);
            _mockRepositoryCommunity.Verify(mrt => mrt.Delete(It.IsAny<Community>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that update CommunityService when data is valid")]
        public void GivenUpdate_WhenDataIsValidNotApprovedAndNew_UpdateCorrectly()
        {
            var contract = new UpdateCommunityContract();
            _mockUpdateCommunityContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>())).Returns(new ValidationResult());
            _mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>())).Returns(new Community());

            _service.Update(contract);

            _mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Exactly(3));
            _mockUpdateCommunityContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>()), Times.Once);
            _mockRepositoryCommunity.Verify(mrt => mrt.Update(It.IsAny<Community>()), Times.Once);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [Fact(DisplayName = "Verify that update throws error when data for updating is invalid")]
        public void GivenUpdate_WhenDataIsInvalid_ThrowCreateContractInvalidException()
        {
            var contract = new UpdateCommunityContract();
            var validationFailure = new ValidationFailure("Title", "IsEmpty");
            _mockUpdateCommunityContractValidator.Setup(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>())).Returns(new ValidationResult(new List<ValidationFailure>() { validationFailure }));
            _mockMapper.Setup(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>())).Returns(new Community());

            var exception = Assert.Throws<Model.Exceptions.Community.CreateContractInvalidException>(() => _service.Update(contract));

            Assert.NotNull(exception);
            Assert.Equal(validationFailure.ErrorMessage, exception.Message);
            _mockLogCommunityService.Verify(mlts => mlts.LogInformation(It.IsAny<string>()), Times.Once);
            _mockUpdateCommunityContractValidator.Verify(utcv => utcv.Validate(It.IsAny<ValidationContext<UpdateCommunityContract>>()), Times.Once);
            _mockMapper.Verify(mm => mm.Map<Community>(It.IsAny<UpdateCommunityContract>()), Times.Never);
            _mockRepositoryCommunity.Verify(mrt => mrt.Update(It.IsAny<Community>()), Times.Never);
            MockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [Fact(DisplayName = "Verify that list returns a value")]
        public void GivenList_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<Community>() { new Community() { Id = 1 } }.AsQueryable();
            var readedCommunityList = new List<ReadedCommunityContract> { new ReadedCommunityContract { Id = 1 } };
            _mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(communities);
            _mockMapper.Setup(mm => mm.Map<List<ReadedCommunityContract>>(It.IsAny<List<Community>>())).Returns(readedCommunityList);

            var actualResult = _service.List();

            Assert.NotNull(actualResult);
            Assert.Equal(1, actualResult.ToList()[0].Id);
            _mockRepositoryCommunity.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<List<ReadedCommunityContract>>(It.IsAny<List<Community>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that read returns a value")]
        public void GivenRead_WhenRegularCall_ReturnsValue()
        {
            var communities = new List<Community>() { new Community() { Id = 1, Name = "Name" } }.AsQueryable();
            var readedCommunity = new ReadedCommunityContract { Id = 1, Name = "Name" };
            _mockRepositoryCommunity.Setup(mrt => mrt.Query()).Returns(communities);
            _mockMapper.Setup(mm => mm.Map<ReadedCommunityContract>(It.IsAny<Community>())).Returns(readedCommunity);

            var actualResult = _service.Read(1);

            Assert.NotNull(actualResult);
            Assert.Equal("Name", actualResult.Name);
            _mockRepositoryCommunity.Verify(_ => _.Query(), Times.Once);
            _mockMapper.Verify(_ => _.Map<ReadedCommunityContract>(It.IsAny<Community>()), Times.Once);
        }
    }
}