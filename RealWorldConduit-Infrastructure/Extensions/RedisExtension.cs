using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealWorldConduit_Infrastructure.Services.Cache;
using StackExchange.Redis;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class RedisExtension
    {
        public static IServiceCollection RedisExtensionConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationOptions = new ConfigurationOptions
            {
                AbortOnConnectFail = true,
                AllowAdmin = true,
                ConnectRetry = 5,
                SyncTimeout = 5000,
            };

            configurationOptions.EndPoints.Add(configuration["Redis:Connection"]);

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = configurationOptions;
                options.InstanceName = configuration["Redis:InstanceName"];
            });

            services.AddScoped<ICacheService, CacheService>();
            return services;
        }
    }
}
