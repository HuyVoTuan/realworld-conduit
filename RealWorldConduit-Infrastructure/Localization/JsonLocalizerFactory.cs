using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace RealWorldConduit_Infrastructure.Localization
{
    internal class JsonLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IMemoryCache _cache;

        public JsonLocalizerFactory(IMemoryCache cache)
        {
            _cache = cache;
        }
        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonLocalizer(_cache);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonLocalizer(_cache);
        }
    }
}
