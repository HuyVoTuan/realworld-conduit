using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection DatabaseExtensionConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("ApplicationDbContext")));
            return services;
        }
    }
}
