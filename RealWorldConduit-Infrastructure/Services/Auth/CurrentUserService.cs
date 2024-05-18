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
                var memberId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                bool isGuidParsed = Guid.TryParse(memberId, out Guid id);

                if (memberId is null || !isGuidParsed)
                {
                    return null;
                }
                return id;
            }
        }
    }
}
