using Domain.Model;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Domain.Services.Repositories.EF.UnitTests
{
    public class SkillTypeRepositoryTest : BaseRepositoryTest
    {
        private readonly SkillTypeRepository _repository;

        public SkillTypeRepositoryTest()
        {
            _repository = new SkillTypeRepository(DbContext, MockUnitOfWork.Object);
        }

        [Fact(DisplayName = "Verify that repository returns null when Query there is no data")]
        public void GivenNotDataInRepositorysDbcontext_WhenQuery_ThenReturnsNull()
        {
            SkillType expectedValue = null;

            var actualValue = _repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(0, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when Query there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQuery_ThenReturnsSkillType()
        {
            var expectedValue = new SkillType();
            DbContext.SkillTypes.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.Query();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
        }

        [Fact(DisplayName = "Verify that repository returns task when QueryEager there is data")]
        public void GivenDataInRepositorysDbcontext_WhenQueryEager_ThenReturnsSkillTypes()
        {
            var expectedValue = new SkillType()
            {
                Skills = new List<Skill>() { new Skill() }
            };
            DbContext.SkillTypes.Add(expectedValue);
            DbContext.SaveChanges();

            var actualValue = _repository.QueryEager();

            Assert.NotNull(actualValue);
            Assert.Equal(1, actualValue.Count());
            Assert.Equal(expectedValue, actualValue.FirstOrDefault());
            Assert.Equal(expectedValue.Skills.Count, actualValue.FirstOrDefault().Skills.Count);
            Assert.Equal(expectedValue.Skills.FirstOrDefault(), actualValue.FirstOrDefault().Skills.FirstOrDefault());
        }
    }
}
