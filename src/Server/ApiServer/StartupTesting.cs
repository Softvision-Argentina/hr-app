using DependencyInjection.Config;
using Domain.Services.Repositories.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ApiServer
{
    public class StartupTesting : Startup
    {
        public new IConfiguration Configuration { get; }
        public new DatabaseConfigurations DatabaseConfigurations { get; set; }
        public StartupTesting(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
        {
            Configuration = configuration;

            //DatabaseConfigurations = new DatabaseConfigurations(false, false, false, string.Empty,
            //    Configuration.GetConnectionString("SeedDBTesting")
            //);

            UseTestingAuthentication = true;
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            //Startup normal configuration
            base.ConfigureServices(services);

            //Startup Testing especial configuration
            var alreadyRegisteredContextService = services.SingleOrDefault(
                _ => _.ServiceType == typeof(DbContextOptions<DataBaseContext>));

            //Override the database context options to replace current connection string for testing connectionstring
            if (alreadyRegisteredContextService != null)
                services.Remove(alreadyRegisteredContextService);

            services.AddDbContext<DataBaseContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);

            //Use always the latest version of migrations
            //using (var db = (DataBaseContext) app.ApplicationServices.GetRequiredService(typeof(DataBaseContext)))
            //{
            //    db.Database.Migrate();
            //}
        }
    }
}
