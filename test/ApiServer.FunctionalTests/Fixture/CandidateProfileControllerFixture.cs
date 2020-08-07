// <copyright file="CandidateProfileControllerFixture.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace ApiServer.FunctionalTests.Fixture
{
    using System.Linq;
    using Core.Testing.Platform;
    using Domain.Model;
    using Microsoft.EntityFrameworkCore;

    public class CandidateProfileControllerFixture : BaseFunctionalTestFixture
    {
        public CandidateProfileControllerFixture()
        {
            this.ControllerName = "CandidateProfile";
        }

        public CandidateProfile GetEager(int id)
        {
            CandidateProfile entity = null;

            this.ContextAction((context) =>
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
            this.Client.Dispose();
            this.Server.Dispose();
        }
    }
}
