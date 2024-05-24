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
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://127.0.0.1:5500")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
