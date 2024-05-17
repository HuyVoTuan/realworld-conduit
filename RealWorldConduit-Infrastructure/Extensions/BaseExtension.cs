using Microsoft.Extensions.DependencyInjection;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class BaseExtension
    {
        public static IServiceCollection BaseExtensionConfig(this IServiceCollection services)
        {
            services.AddLogging();
            return services;
        }
    }
}
