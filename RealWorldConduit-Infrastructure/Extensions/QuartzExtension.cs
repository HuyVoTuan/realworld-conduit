using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using RealWorldConduit_Infrastructure.Services.Jobs;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class QuartzExtension
    {
        public static IServiceCollection QuartzExtensionConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddQuartz(opt =>
            {
                opt.UseTimeZoneConverter();
                opt.UseSimpleTypeLoader();
                opt.UsePersistentStore(_opt =>
                {
                    _opt.UseClustering();
                    _opt.UsePostgres(config =>
                    {
                        config.ConnectionString = configuration.GetConnectionString("MainDbContext");
                    });
                    _opt.UseNewtonsoftJsonSerializer();
                });
                opt.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 5;
                });
            });

            services.AddScoped<IJobService, JobService>();
            return services;
        }
    }
}
