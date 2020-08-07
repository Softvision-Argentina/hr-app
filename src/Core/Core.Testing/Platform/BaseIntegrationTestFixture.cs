// <copyright file="BaseIntegrationTestFixture.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Testing.Platform
{
    public class BaseIntegrationTestFixture : WebAppFactory
    {
        public BaseIntegrationTestFixture()
        {
            this.ContextAction((context) =>
            {
                // context.ResetAllIdentitiesId();
            });
        }
    }
}
