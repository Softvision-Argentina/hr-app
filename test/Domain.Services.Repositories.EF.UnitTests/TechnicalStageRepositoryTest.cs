﻿namespace Domain.Services.Repositories.EF.UnitTests
{
    using System;
    using System.Linq;
    using Domain.Model;
    using Xunit;

    public class TechnicalStageRepositoryTest : BaseRepositoryTest
    {
        private readonly TechnicalStageRepository repository;

        public TechnicalStageRepositoryTest()
        {
            this.repository = new TechnicalStageRepository(this.DbContext, this.MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when QueryEager there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            TechnicalStage expectedValue = null;

            var actualValue = this.repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns TechnicalStage when QueryEager there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsTechnicalStage()
        {
            var userDelegate = new User();
            var userOwner = new User();
            var expectedValue = new TechnicalStage()
            {
                UserDelegate = userDelegate,
                UserOwner = userOwner,
            };
            this.DbContext.TechnicalStages.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.UserDelegate, actualValue.FirstOrDefault().UserDelegate);
            Assert.Equal(expectedValue.UserOwner, actualValue.FirstOrDefault().UserOwner);
        }

        [Fact(DisplayName = "Verify that repository returns user when QueryEager there is data", Skip = "There are no calls to UpdateTechnicalStage")]
        public void GivenUsersInRepositorysDbcontext_WhenQueryEager_ThenReturnsUser()
        {
            var existingStage = new TechnicalStage()
            {
                AlternativeSeniority = Model.Enum.Seniority.Junior1,
                Client = "client1",
                UserDelegate = new User(),
                UserDelegateId = 0,
                UserOwner = new User(),
                UserOwnerId = 0,
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
                Version = 1,
            };
            var newStage = new TechnicalStage()
            {
                AlternativeSeniority = Model.Enum.Seniority.Junior2,
                Client = "client2",
                UserDelegate = new User(),
                UserDelegateId = 2,
                UserOwner = new User(),
                UserOwnerId = 2,
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
                Version = 2,
            };
            this.DbContext.TechnicalStages.Add(existingStage);
            this.DbContext.SaveChanges();

            this.repository.UpdateTechnicalStage(newStage, existingStage);

            Assert.NotNull(this.DbContext.Entry(existingStage));
        }
    }
}
