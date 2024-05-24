
using Microsoft.EntityFrameworkCore;
using RealWorldConduit_Domain.Constants;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure;

namespace RealWorldConduit_API.HostedJob
{
    public class CreateCountryRecordsJob : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CreateCountryRecordsJob> _logger;
        public CreateCountryRecordsJob(IServiceScopeFactory scopeFactory, ILogger<CreateCountryRecordsJob> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var isCountryRecordsExist = await dbContext.Countries.AnyAsync(cancellationToken);

                if (isCountryRecordsExist)
                {
                    _logger.LogInformation($"[JOB-{typeof(CreateCountryRecordsJob).Name}] Country records are already exists.");
                }
                else
                {
                    await dbContext.Countries.AddRangeAsync(new List<Country>
                    {
                        new Country
                        {
                            Code = CountryCode.vietnamCode,
                            Language = CountryCode.vietnameseCulture,
                        },
                         new Country
                        {
                            Code = CountryCode.usaCode,
                            Language = CountryCode.englishCulture
                        }
                    }, cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
