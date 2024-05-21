using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Commands
{
    public record UserDeleteLocationCommand(string Slug) : IRequestWithBaseResponse;

    internal class UserDeleteLocationCommandHandler : IRequestWithBaseResponseHandler<UserDeleteLocationCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<UserDeleteLocationCommandHandler> _localizer;

        public UserDeleteLocationCommandHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<UserDeleteLocationCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse> Handle(UserDeleteLocationCommand request, CancellationToken cancellationToken)
        {
            var existingUserLocation = await _dbContext.Locations.FirstOrDefaultAsync(x => x.Slug == request.Slug && x.UserId == _currentUser.Id, cancellationToken);

            if (existingUserLocation is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            _dbContext.Locations.Remove(existingUserLocation);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse(HttpStatusCode.NoContent);
        }
    }
}
