using Domain.Model;
using System;
using System.Linq;
using Xunit;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class TechnicalStageRepositoryTest : BaseRepositoryTest
    {
        private readonly TechnicalStageRepository _repository;

        public TechnicalStageRepositoryTest()
        {
            _repository = new TechnicalStageRepository(DbContext, MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when QueryEager there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            TechnicalStage expectedValue = null;

            var actualValue = _repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns TechnicalStage when QueryEager there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsTechnicalStage()
        {
            var consultantDelegate = new Consultant();
            var consultantOwner = new Consultant();
            var expectedValue = new TechnicalStage()
            {
                ConsultantDelegate = consultantDelegate,
                ConsultantOwner = consultantOwner
            };
            DbContext.TechnicalStages.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.ConsultantDelegate, actualValue.FirstOrDefault().ConsultantDelegate);
            Assert.Equal(expectedValue.ConsultantOwner, actualValue.FirstOrDefault().ConsultantOwner);
        }

        [Fact(DisplayName = "Verify that repository returns user when QueryEager there is data", Skip = "There are no calls to UpdateTechnicalStage")]
        public void GivenUsersInRepositorysDbcontext_WhenQueryEager_ThenReturnsUser()
        {
            var existingStage = new TechnicalStage()
            {
                AlternativeSeniority = Model.Enum.Seniority.Junior1,
                Client = "client1",
                ConsultantDelegate = new Consultant(),
                ConsultantDelegateId = 0,
                ConsultantOwner = new Consultant(),
                ConsultantOwnerId = 0,
                CreatedBy = "creator",
                CreatedDate = DateTime.Today,
                Date = DateTime.Today,
                Feedback = "feedback",
                Id = 0,
                LastModifiedBy = "creator",
                LastModifiedDate = DateTime.Today,
                Process = new Process(),
                ProcessId = 0,
                RejectionReason = "reason1",
                Seniority = Model.Enum.Seniority.Junior1,
                Status = Model.Enum.StageStatus.Accepted,
                Type = Model.Enum.StageType.Client,
                Version = 1
            };
            var newStage = new TechnicalStage()
            {
                AlternativeSeniority = Model.Enum.Seniority.Junior2,
                Client = "client2",
                ConsultantDelegate = new Consultant(),
                ConsultantDelegateId = 2,
                ConsultantOwner = new Consultant(),
                ConsultantOwnerId = 2,
                CreatedBy = "creator",
                CreatedDate = DateTime.Today,
                Date = DateTime.Today,
                Feedback = "feedback1",
                Id = 0,
                LastModifiedBy = "modifier",
                LastModifiedDate = DateTime.Today.AddDays(-1),
                Process = new Process(),
                ProcessId = 1,
                RejectionReason = "reason2",
                Seniority = Model.Enum.Seniority.Junior2,
                Status = Model.Enum.StageStatus.Declined,
                Type = Model.Enum.StageType.Hire,
                Version = 2
            };
            DbContext.TechnicalStages.Add(existingStage);
            DbContext.SaveChanges();
            
            _repository.UpdateTechnicalStage(newStage, existingStage);

            Assert.NotNull(DbContext.Entry(existingStage));
        }
    }
}
