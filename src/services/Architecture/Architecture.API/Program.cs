using Autofac.Extensions.DependencyInjection;
using Architecture.Infrastruture;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Events;
using System;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Architecture.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.File(@"/app/log.txt", fileSizeLimitBytes: 1_000_000, rollOnFileSizeLimit: true, shared: true, outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] ({FilePath}{SourceContext}.{MemberName}:{LineNumber}) {NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", flushToDiskInterval: TimeSpan.FromSeconds(1)).CreateLogger();

            try
            {
                Log.Information("Starting up");

                var host = CreateHostBuilder(args)
                    .Build()
                    .MigrateDbContext<ArchitectureContext>((context, services) =>
                {

                    var env = services.GetService<IHostingEnvironment>();
                    var logger = services.GetService<ILogger<ArchitectureContextSeed>>();
                    
                    new ArchitectureContextSeed().SeedAsync(context, env, logger).Wait();

                });

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var builtConfig = config.Build();
                    var configurationBuilder = new ConfigurationBuilder();
                    configurationBuilder.AddEnvironmentVariables();
                    config.AddConfiguration(configurationBuilder.Build());
                })
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .UseSerilog((ctx, config) =>
                {
                    config.MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .MinimumLevel.Override("System", LogEventLevel.Warning)
                            .Enrich.FromLogContext();

                    if (ctx.HostingEnvironment.IsDevelopment())
                    {
                        config.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] ({FilePath}{SourceContext}.{MemberName}:{LineNumber}) {NewLine}{Message:lj}{NewLine}{Exception}{NewLine}");
                    }
                    else if (ctx.HostingEnvironment.IsProduction())
                    {
                        config
                        .WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces)
                        .WriteTo.File(@"/app/log.txt",
                            fileSizeLimitBytes: 1_000_000,
                            rollOnFileSizeLimit: true,
                            shared: true,
                            outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] ({FilePath}{SourceContext}.{MemberName}:{LineNumber}) {NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                            flushToDiskInterval: TimeSpan.FromSeconds(1));
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<Startup>();
                });
    }
}
