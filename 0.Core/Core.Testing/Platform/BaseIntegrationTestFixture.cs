using System;
using System.Collections.Generic;
using System.Linq;
using Core.Testing.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance.EF.Extensions;

namespace Core.Testing.Platform
{
    public class BaseIntegrationTestFixture : WebAppFactory
    {
        public BaseIntegrationTestFixture()
        {
            ContextAction((context) =>
            {
               // context.ResetAllIdentitiesId();
            });
        }
    }
}
