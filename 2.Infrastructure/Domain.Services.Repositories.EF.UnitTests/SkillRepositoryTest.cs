using Domain.Model;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class SkillRepositoryTest : BaseRepositoryTest
    {
        private readonly SkillRepository _repository;

        public SkillRepositoryTest()
        {
            _repository = new SkillRepository(DbContext, MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            Skill expectedValue = null;

            var actualValue = _repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsSkill()
        {
            var expectedValue = new Skill();
            DbContext.Skills.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.Query();

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
                Type = new SkillType()
            };
            DbContext.Skills.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.CandidateSkills, actualValue.FirstOrDefault().CandidateSkills);
            Assert.Equal(expectedValue.Type, actualValue.FirstOrDefault().Type);
        }
    }
}
