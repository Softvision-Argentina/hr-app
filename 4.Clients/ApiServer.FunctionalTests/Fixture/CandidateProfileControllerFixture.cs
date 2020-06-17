using System.Linq;
using System.Threading.Tasks;
using Core.Testing.Platform;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace ApiServer.FunctionalTests.Fixture
{
    public class CandidateProfileControllerFixture : BaseFunctionalTestFixture
    {
        public CandidateProfileControllerFixture()
        {
            ControllerName = "CandidateProfile";
        }

        public CandidateProfile GetEager(int id)
        {
            CandidateProfile entity = null;

            ContextAction((context) =>
            {
                entity = context.Set<CandidateProfile>()
                    .Include(_ => _.CommunityItems)
                    .AsNoTracking()
                    .Single(_ => _.Id == id);
            });

            return entity;
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}

