// <copyright file="CommunityControllerFixture.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.FunctionalTests.Fixture
{
    using Core.Testing.Platform;

    public class CommunityControllerFixture : BaseFunctionalTestFixture
    {
        public CommunityControllerFixture()
        {
            this.ControllerName = "Community";
        }

        public void Dispose()
        {
            this.Client.Dispose();
            this.Server.Dispose();
        }
    }
}
