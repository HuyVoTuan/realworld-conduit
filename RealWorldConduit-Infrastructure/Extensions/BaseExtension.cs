using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class BaseExtension
    {
        public static IServiceCollection BaseExtensionConfig(this IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });
            services.AddMemoryCache();
            return services;
        }
    }
}
