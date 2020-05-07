using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApiServer.FunctionalTests.Core;
using Domain.Model;
using Task = System.Threading.Tasks.Task;

namespace ApiServer.FunctionalTests.Fixture
{
    public class CandidateProfileControllerFixture : BaseFunctionalTestFixture
    {
        public CandidateProfileControllerFixture()
        {
            ControllerName = "CandidateProfile";
        }
        public override async Task<object> GetEagerAsync(int id)
        {
            var model = Context.Set<CandidateProfile>()
                .Include(_ => _.CommunityItems)
                .AsNoTracking()
                .Single(_ => _.Id == id);

            return await Task.FromResult((object) model);
        }
        public void Dispose()
        {
            Context.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}

