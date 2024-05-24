using Microsoft.EntityFrameworkCore;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Helpers;
using RealWorldConduit_Infrastructure.Services.Auth;

namespace RealWorldConduit_API.HostedJob
{
    public class CreatePersonalUserJob : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CreatePersonalUserJob> _logger;

        private readonly string _personalUserEmail;
        private readonly string _personalUserUsername;
        private readonly string _personalUserPassword;

        public CreatePersonalUserJob(IServiceScopeFactory scopeFactory, ILogger<CreatePersonalUserJob> logger, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            _personalUserEmail = configuration["PersonalUser:Email"];
            _personalUserUsername = configuration["PersonalUser:Username"];
            _personalUserPassword = configuration["PersonalUser:Password"];
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var existingUser = await dbContext.Users.AsNoTracking()
                                                        .FirstOrDefaultAsync(u => u.Email == _personalUserEmail);

                if (existingUser is not null)
                {
                    _logger.LogInformation($"[JOB-{typeof(CreatePersonalUserJob).Name}] Personal user with email {_personalUserEmail} is already exists.");
                }
                else
                {
                    var newUser = new User
                    {
                        Slug = StringHelper.GenerateSlug($"{_personalUserUsername}"),
                        Email = _personalUserEmail,
                        Username = _personalUserUsername,
                        Password = auth.HashPassword(_personalUserPassword),
                        isActive = true
                    };

                    await dbContext.Users.AddAsync(newUser, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
