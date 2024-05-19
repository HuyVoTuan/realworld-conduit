using Microsoft.Extensions.DependencyInjection;
using RealWorldConduit_Infrastructure.Behaviors;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class MediatRExtension
    {
        public static IServiceCollection MediatRExtensionConfig(this IServiceCollection services)
        {
            var executingAssembly = AppDomain.CurrentDomain.Load("RealWorldConduit-Application");
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(executingAssembly);
                // MediatR Pipelines
                cfg.AddOpenBehavior(typeof(MediatRPipelineBehavior<,>), ServiceLifetime.Singleton);
            });


            return services;
        }
    }
}
