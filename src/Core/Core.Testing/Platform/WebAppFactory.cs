// <copyright file="WebAppFactory.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Core.Testing.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using ApiServer;
    using Core.Testing.Interfaces;
    using Domain.Services.Repositories.EF;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class WebAppFactory : ITestingHelpers
    {
        public TestServer Server { get; set; }

        public HttpClient Client { get; set; }

        public WebAppFactory()
        {
            var builder = TestHelpers.GetConfigurationBuilder();
            this.Server = TestHelpers.GetTestServer<StartupTesting>(builder);
            this.Client = this.Server.CreateClient();
        }

        // ITestingHelpers
        public void ContextAction(Action<DataBaseContext> action)
        {
            using (var scope = this.Server.Host.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = (DataBaseContext)scope.ServiceProvider.GetService(typeof(DataBaseContext));
                {
                    action(context);
                }
            }
        }

        public void UseService<T>(Action<T> action) where T : class
        {
            using (var scope = this.Server.Host.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var service = (T)scope.ServiceProvider.GetService(typeof(T));
                {
                    action(service);
                }
            }
        }

        public void Seed<T>(List<T> entities) where T : class
        {
            this.ContextAction((context) =>
            {
                entities.ForEach((entity) => { context.Set<T>().Add(entity); });
                context.SaveChanges();

                // context.DetachAllEntities();T
            });
        }

        public void Seed<T>(T entity) where T : class
        {
            this.ContextAction((context) =>
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
                context.Entry(entity).State = EntityState.Detached;
            });
        }

        public T Get<T>(int id) where T : Entity<int>
        {
            T entity = null;

            this.ContextAction((context) =>
            {
                entity = context.Set<T>().AsNoTracking().First(_ => _.Id == id);
            });

            return entity;
        }

        public int GetCount<T>() where T : class
        {
            int count = 0;

            this.ContextAction((context) =>
            {
                count = context.Set<T>().AsNoTracking().ToList().Count;
            });

            return count;
        }

        public virtual T GetEager<T>(int id) where T : class
        {
            throw new NotImplementedException();
        }

        // public void CleanTestingDatabase()
        // {
        //    ContextAction((context) =>
        //    {
        //        context.DeleteAllEntities();
        //    });
        // }
    }
}
