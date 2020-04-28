using DependencyInjection;
using DependencyInjection.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment { get; }

        public IConfiguration Configuration { get; }

        public DatabaseConfigurations DatabaseConfigurations { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;

            DatabaseConfigurations = new DatabaseConfigurations(
                Configuration.GetValue("InMemoryDatabase", true) && hostingEnvironment.CanModifyScheme(),
                Configuration.GetValue("RunMigrations", false) && hostingEnvironment.CanModifyScheme(),
                Configuration.GetValue("RunSeed", false) && hostingEnvironment.CanModifyScheme(),
                Configuration.GetConnectionString("SeedDB")
            );
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });

            services.AddLogging();

            services.AddDomain(DatabaseConfigurations);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action=Index}/{id?}");
            });


            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }

    public static class HostingExtensions
    {
        public static bool CanModifyScheme(this IHostingEnvironment env)
        {
            return env.IsDevelopment() ||
                   env.IsEnvironment("INT") ||
                   env.IsEnvironment("IntegrationTest") ||
                   env.IsEnvironment("FunctionalTest");
        }
    }
}
