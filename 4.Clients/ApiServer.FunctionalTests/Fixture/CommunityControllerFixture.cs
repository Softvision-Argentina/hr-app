using ApiServer.FunctionalTests.Core;
using Persistance.EF.Extensions;
using System.Threading.Tasks;
using Xunit;

namespace ApiServer.FunctionalTests.Fixture
{
    public class CommunityControllerFixture : BaseFunctionalTestFixture
    {
        public CommunityControllerFixture()
        {
            ControllerName = "Community";
        }

        public void Dispose()
        {
            Context.Dispose();
            Client.Dispose();
            Server.Dispose();
        }
    }
}
