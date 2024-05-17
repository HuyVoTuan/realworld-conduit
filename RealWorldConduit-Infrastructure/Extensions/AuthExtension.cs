using Microsoft.Extensions.DependencyInjection;
using RealWorldConduit_Infrastructure.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AuthExtensionConfig(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
