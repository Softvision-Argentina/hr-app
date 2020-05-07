using ApiServer.FunctionalTests.Core;

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
