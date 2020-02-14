using ApiServer.Security;
using ApiServer.Security.AuthenticationTest;
using Core.Persistance;
using DependencyInjection;
using DependencyInjection.Config;
using Domain.Services.ExternalServices.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer
{
    public class Startup
    {
        private IHostingEnvironment _env { get; }
        public IConfiguration Configuration { get; }
        public DatabaseConfigurations DatabaseConfigurations { get; set; }

        public bool UseTestingAuthentication { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;

            DatabaseConfigurations = new DatabaseConfigurations(
                Configuration.GetValue("InMemoryDatabase", true) && env.CanModifyScheme(),
                Configuration.GetValue("RunMigrations", false) && env.CanModifyScheme(),
                Configuration.GetValue("RunSeed", false) && env.CanModifyScheme(),
                Configuration.GetConnectionString("SeedDB")
            );

            UseTestingAuthentication = Configuration.GetValue("UseTestAuthentication", false);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = new JwtSettings
            {
                Key = Configuration["jwtSettings:key"],
                Issuer = Configuration["jwtSettings:issuer"],
                Audience = Configuration["jwtSettings:audience"],
                MinutesToExpiration = int.Parse(Configuration["jwtSettings:minutesToExpiration"])
            };
            services.AddSingleton(jwtSettings);

            if (UseTestingAuthentication)
            {
                services
                    .AddAuthentication(TestAuthenticationOptions.AuthenticationScheme)
                    .AddTestAuthentication();
            }
            else
            {
                #region original
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                })                
                .AddJwtBearer("JwtBearer", jwtBearerOptions =>
                {
                    
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(jwtSettings.MinutesToExpiration)
                    };
                });
                #endregion
            }

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy(SecurityClaims.CAN_LIST_DUMMY, p =>
                   p.RequireClaim(SecurityClaims.CAN_LIST_DUMMY, "true"));

                cfg.AddPolicy(SecurityClaims.CAN_LIST_CANDIDATE, p =>
                    p.RequireClaim(SecurityClaims.CAN_LIST_CANDIDATE, "true"));
            });

            services.AddCors();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add framework services.
            services.AddMvc()
                    .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); 

            services.AddLogging();

            services.AddDomain(DatabaseConfigurations);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //This is not working
            //loggerFactory.AddFile(Configuration.GetSection("Logging"));

            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            app.UseCors((option) =>
                option.WithOrigins(Configuration["corsWhiteList"].Split(','))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseCookiePolicy();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var migrator = serviceScope.ServiceProvider.GetService<IMigrator>();
                migrator.Migrate(DatabaseConfigurations);
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }

    public static class HostingExtensions
    {
        public static bool CanModifyScheme(this IHostingEnvironment env)
        {
            return env.IsDevelopment() || env.IsEnvironment("INT") || env.IsEnvironment("IntegrationTest");
        }
    }
}
