using Microsoft.EntityFrameworkCore;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Commands
{
    public record UserRevokeTokenCommand() : IRequestWithBaseResponse;
    internal class UserRevokeTokenCommandHandler : IRequestWithBaseResponseHandler<UserRevokeTokenCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;

        public UserRevokeTokenCommandHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse> Handle(UserRevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenLists = await _dbContext.RefreshTokens.Where(x => x.UserId == _currentUser.Id).ToListAsync(cancellationToken);

            _dbContext.RefreshTokens.RemoveRange(refreshTokenLists);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse(HttpStatusCode.NoContent);
        }
    }
}
