using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Infrastructure.Localization;
using System.Globalization;

namespace RealWorldConduit_Infrastructure.Extensions
{
    public static class LocalizerExtension
    {
        public static IServiceCollection LocalizerExtensionConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization();

            var jsonLocalizationOptions = configuration.GetSection(nameof(JsonLocalizerOptions)).Get<JsonLocalizerOptions>();
            var supportedCultureInfos = jsonLocalizationOptions.SupportedCultureInfos.ToList();

            services.Configure<RequestLocalizationOptions>(opt =>
            {
                opt.SetDefaultCulture(jsonLocalizationOptions.DefaultCulture);
                opt.SupportedCultures = supportedCultureInfos.Select(x => new CultureInfo(x)).ToList();
            });

            services.AddScoped<IStringLocalizer, JsonLocalizer>();
            services.AddSingleton<IStringLocalizerFactory, JsonLocalizerFactory>();
            return services;
        }

        public static String Translate(this IStringLocalizer<dynamic> localizer, String key)
        {
            var result = localizer[key.ToLower()];
            return !result.ResourceNotFound ? result.Value : key;
        }

        public static String Translate(this IStringLocalizer<dynamic> localizer, String key, List<String> args)
        {
            var result = localizer[key.ToLower(), args.ToArray()];
            return !result.ResourceNotFound ? result.Value : key;
        }
    }
}
