using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace RealWorldConduit_Infrastructure.Services.Auth
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid? Id
        {
            get
            {
                var currentId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool isGuidParsed = Guid.TryParse(currentId, out Guid id);

                if (currentId is null || !isGuidParsed)
                {
                    return null;
                }
                return id;
            }
        }

        public string? Slug
        {
            get
            {
                var currentSlug = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

                if (String.IsNullOrEmpty(currentSlug))
                {
                    return null;
                }

                return currentSlug;
            }
        }
    }
}
