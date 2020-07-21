using ApiServer.Security;
using ApiServer.Security.AuthenticationTest;
using Core.Persistance;
using DependencyInjection;
using DependencyInjection.Config;
using Domain.Services.ExternalServices.Config;
using Mailer;
using Mailer.Entities;
using Mailer.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MimeKit;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ApiServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public DatabaseConfigurations DatabaseConfigurations { get; set; }
        public bool UseTestingAuthentication { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            DatabaseConfigurations = new DatabaseConfigurations(
                Configuration.GetValue("InMemoryDatabase", false),
                Configuration.GetValue("RunMigrations", false),
                Configuration.GetValue("RunSeed", false),
                Configuration.GetConnectionString("SeedDB")
            );

            UseTestingAuthentication = Configuration.GetValue("UseTestAuthentication", false);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
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
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddLogging();

            services.AddDomain(DatabaseConfigurations);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Recru API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement {
                        { new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme, Id = "Bearer"
                            }
                        },
                            new string[] {}
                        }
                    });
            });

            var mailConfig = Configuration.GetSection("MailSettings")
                .Get<MailServerSettings>();

            services.AddSingleton(mailConfig);
            services.AddScoped<IMailSender, MailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed((host) => true)
            );

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseCookiePolicy();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var migrator = serviceScope.ServiceProvider.GetService<IMigrator>();
                migrator.Migrate(DatabaseConfigurations);
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
