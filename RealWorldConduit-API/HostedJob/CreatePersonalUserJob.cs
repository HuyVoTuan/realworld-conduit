﻿
using DotLiquid;
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
        private readonly string _personalUserFirstName;
        private readonly string _personalUserLastName;
        private readonly string _personalUserPassword;

        public CreatePersonalUserJob(IServiceScopeFactory scopeFactory, ILogger<CreatePersonalUserJob> logger, IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            _personalUserEmail = configuration["PersonalUser:Email"];
            _personalUserFirstName = configuration["PersonalUser:Firstname"];
            _personalUserLastName = configuration["PersonalUser:Lastname"];
            _personalUserPassword = configuration["PersonalUser:Password"];
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();

                var existingUser = await dbContext.Users.AsNoTracking()
                                                     .FirstOrDefaultAsync(u => u.Email == _personalUserEmail);

                if (existingUser is not null)
                {
                    _logger.LogInformation($"Personal user with email {_personalUserEmail} already exists.");
                }
                else
                {
                    var newUser = new User
                    {
                        Slug = StringHelper.GenerateSlug($"{_personalUserFirstName} {_personalUserLastName}"),
                        Email = _personalUserEmail,
                        Firstname = _personalUserFirstName,
                        Lastname = _personalUserLastName,
                        Password = auth.HashPassword(_personalUserPassword)
                    };

                    await dbContext.Users.AddAsync(newUser, cancellationToken);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}