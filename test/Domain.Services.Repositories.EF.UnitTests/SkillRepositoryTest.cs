namespace Domain.Services.Repositories.EF.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Model;
    using Xunit;

    public class SkillRepositoryTest : BaseRepositoryTest
    {
        private readonly SkillRepository repository;

        public SkillRepositoryTest()
        {
            this.repository = new SkillRepository(this.DbContext, this.MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            Skill expectedValue = null;

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsSkill()
        {
            var expectedValue = new Skill();
            this.DbContext.Skills.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when QueryEager there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQueryEager_ThenReturnsSkill()
        {
            var expectedValue = new Skill()
            {
                CandidateSkills = new List<CandidateSkill>(),
                Type = new SkillType(),
            };
            this.DbContext.Skills.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.CandidateSkills, actualValue.FirstOrDefault().CandidateSkills);
            Assert.Equal(expectedValue.Type, actualValue.FirstOrDefault().Type);
        }
    }
}
