namespace Domain.Services.Repositories.EF.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Model;
    using Xunit;

    public class SkillTypeRepositoryTest : BaseRepositoryTest
    {
        private readonly SkillTypeRepository repository;

        public SkillTypeRepositoryTest()
        {
            this.repository = new SkillTypeRepository(this.DbContext, this.MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            SkillType expectedValue = null;

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsSkillType()
        {
            var expectedValue = new SkillType();
            this.DbContext.SkillTypes.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when QueryEager there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQueryEager_ThenReturnsSkillTypes()
        {
            var expectedValue = new SkillType()
            {
                Skills = new List<Skill>() { new Skill() },
            };
            this.DbContext.SkillTypes.Add(expectedValue);
            this.DbContext.SaveChanges();

            var actualValue = this.repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.Skills.Count, actualValue.FirstOrDefault().Skills.Count);
            Assert.Equal(expectedValue.Skills.FirstOrDefault(), actualValue.FirstOrDefault().Skills.FirstOrDefault());
        }
    }
}
