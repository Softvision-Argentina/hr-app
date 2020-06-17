using Core.Testing.Platform;

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
            Client.Dispose();
            Server.Dispose();
        }
    }
}
