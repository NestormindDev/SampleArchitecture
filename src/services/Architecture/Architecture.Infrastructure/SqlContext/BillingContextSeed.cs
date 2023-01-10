
using Architecture.Domain.AggregatesModel; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Architecture.Infrastruture
{
    public class ArchitectureContextSeed
    {
        public async Task SeedAsync(ArchitectureContext context, IHostingEnvironment env, ILogger<ArchitectureContextSeed> logger)
        {
            var _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var policy = CreatePolicy(logger, nameof(ArchitectureContextSeed));
            await policy.ExecuteAsync(async () =>
            {
                try
                {

                    if (_env == "Development")
                    {
                        if (!context.Countries.Any())
                        {
                            await context.Countries.AddRangeAsync(GetPreconfiguredCountries());

                            await context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<ArchitectureContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<Exception>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                    }
                );
        }
        private IEnumerable<Country> GetPreconfiguredCountries()
        {
            return new List<Country>()
            {
                new Country("Canada", "CA"),
                new Country("United States", "USA")
            };
        }
    }
}
