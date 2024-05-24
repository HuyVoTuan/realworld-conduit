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
    public record UserDeactivateCommand() : IRequestWithBaseResponse;

    internal class UserDeactivateCommandHandler : IRequestWithBaseResponseHandler<UserDeactivateCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<UserDeactivateCommandHandler> _localizer;

        public UserDeactivateCommandHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<UserDeactivateCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse> Handle(UserDeactivateCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == _currentUser.Id &&
                                                                          x.isActive == true, cancellationToken);

            if (existingUser is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            existingUser.isActive = false;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse(HttpStatusCode.NoContent);
        }
    }
}
