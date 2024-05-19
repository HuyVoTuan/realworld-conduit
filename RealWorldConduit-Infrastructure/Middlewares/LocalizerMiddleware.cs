using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RealWorldConduit_Infrastructure.Localization;
using System.Globalization;

namespace RealWorldConduit_Infrastructure.Middlewares
{
    public class LocalizerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public LocalizerMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var language = context.Request.Headers["Accept-Language"].ToString();

            var jsonLocalizationOptions = _configuration.GetSection(nameof(JsonLocalizerOptions)).Get<JsonLocalizerOptions>();
            var supportedCultureInfos = jsonLocalizationOptions.SupportedCultureInfos.ToList();

            var cultureKey = supportedCultureInfos.FirstOrDefault(x => x.StartsWith(language));

            if (cultureKey is null)
            {
                cultureKey = jsonLocalizationOptions.DefaultCulture;
            }

            var applicationCulture = new CultureInfo(cultureKey);
            Thread.CurrentThread.CurrentCulture = applicationCulture;
            Thread.CurrentThread.CurrentUICulture = applicationCulture;

            await _next(context);
        }
    }
}
