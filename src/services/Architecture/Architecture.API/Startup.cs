using Autofac;
using AutoMapper;
using Architecture.API.Infrastructure.AutofacModules;
using Architecture.Infrastruture;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using Architecture.API.Infrastructure.Auth;
using Architecture.Infrastructure.Helpers;
using Review.Code.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Architecture.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region --JWT settings preparation--
            JWTSettings jwtSettings = new JWTSettings();
            Configuration.Bind(ConfigurationKeyConstant.JWT_SETTINGS, jwtSettings);
            services.AddSingleton(jwtSettings);
            #endregion --JWT settings preparation--

            #region --JWT and authentication/authorization configuration--
            ///adding jwt 
            //var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    //TODO: We need to get the key from the key manager
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration[ConfigurationKeyConstant.JWT_SECRET_KEY]))
                };
            });
            #endregion --JWT and authentication/authorization configuration--

            services.AddAuthorization(options =>
            {
                // Create policy to check for the scope 'read' and 'write'
                options.AddPolicy("ReadScope", policy => policy.Requirements.Add(new ScopesRequirement("data.read")));
                options.AddPolicy("WriteScope", policy => policy.Requirements.Add(new ScopesRequirement("data.write")));
            });

            //The specified URL must not contain a trailing slash(/)
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });


            //Infrastructure Service Registration
            services
                .AddCustomMvc()
                .AddCustomSwagger(Configuration)
                .AddApplicationInsights(Configuration)
                .AddHealthChecks(Configuration)
                .AddCustomIntegrations(Configuration)
                .AddCustomConfiguration(Configuration)
                .AddCustomDbContext(Configuration);

            services.AddOptions();

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Mediator Service Registration
            builder.RegisterModule(new MediatorModule());

            //Application Service Registration (Queries, Repositories) :
            string dbConnStr = Configuration["ConnectionString"];

            builder.RegisterModule(new ApplicationModule(dbConnStr));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If, for some reason, you need a reference to the built container, you

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Architecture.API");
                c.RoutePrefix = string.Empty;
            });



            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins); //must be placed after UseRouting, before UseAuthorization

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry();
            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddScoped<JwtHandler>();

            //services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration["ConnectionString"];
            string migrationAssemblyName = "Architecture.Infrastructure"; // typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ArchitectureContext>(options =>
            {
                options.UseSqlServer(connectionString,
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(migrationAssemblyName);
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                         sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory");
                                     })
                                     .EnableDetailedErrors()
                                     .EnableSensitiveDataLogging();
            });
            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Sample - Architecture",
                    Description = "Web API",
                });
            });

            return services;
        }

        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            return services;
        }

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<ApiSettings>(configuration);
            return services;
        }




    }
}
