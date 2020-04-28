using System;
using Domain.Services.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApiServer.FunctionalTests.Core;
using Core;
using Domain.Model;
using Microsoft.IdentityModel.Xml;
using Persistance.EF.Extensions;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ApiServer.FunctionalTests.Fixture
{
    public class CandidateProfileControllerFixture : BaseFunctionalTestFixture
    {
        public CandidateProfileControllerFixture()
        {
            ControllerName = "CandidateProfile";
        }

        public override async Task DeleteEagerAsync()
        {
            Context.DetachAllEntities();

            var list = Context
                .Profiles
                .AsNoTracking()
                .Include(_ => _.CommunityItems);

            var model = await list.ToListAsync();

            model.ForEach((entity) => { Context.Remove(entity); });

            await Context.SaveChangesAsync();
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

