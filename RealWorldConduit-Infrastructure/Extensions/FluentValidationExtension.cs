using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class FluentValidationExtension
    {
        public static IServiceCollection FluentValidationConfig(this IServiceCollection services)
        {
            var executingAssembly = AppDomain.CurrentDomain.Load("RealWorldConduit-Application");

            // Base validation base on Fluent Validation Behavior Pipeline
            services.AddValidatorsFromAssembly(executingAssembly).AddFluentValidationAutoValidation();

            return services;
        }
    }
}
