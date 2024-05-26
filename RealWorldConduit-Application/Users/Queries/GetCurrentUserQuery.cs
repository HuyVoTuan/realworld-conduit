using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Queries
{
    public record GetCurrentUserQuery() : IRequestWithBaseResponse<DetailUserDTO>;
    internal class GetCurrentUserQueryHandler : IRequestWithBaseResponseHandler<GetCurrentUserQuery, DetailUserDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<GetCurrentUserQueryHandler> _localizer;

        public GetCurrentUserQueryHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<GetCurrentUserQueryHandler> localizer)
        {
            _localizer = localizer;
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<DetailUserDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _dbContext.Users.AsNoTracking()
                                                    .Include(x => x.UsersThatFollow)
                                                        .ThenInclude(y => y.UserBeingFollowed)
                                                    .FirstOrDefaultAsync(x => x.Id == _currentUser.Id, cancellationToken);

            var currentUserDTO = new DetailUserDTO
            {
                Slug = currentUser.Slug,
                Username = currentUser.Username,
                AvatarUrl = currentUser.AvatarUrl,
                Bio = currentUser.Bio,
                Email = currentUser.Email,
                Friends = currentUser.UsersThatFollow.Select(x => new MinimalUserDTO
                {
                    Slug = x.UserBeingFollowed.Slug,
                    Username = x.UserBeingFollowed.Username,
                    AvatarUrl = x.UserBeingFollowed.AvatarUrl,
                    Bio = x.UserBeingFollowed.Bio,
                    Email = x.UserBeingFollowed.Email,
                    IsFollowing = x.UserThatFollowId == _currentUser.Id,
                    CreatedDate = x.UserBeingFollowed.CreatedDate,
                    UpdatedDate = x.UserBeingFollowed.UpdatedDate,
                }).ToList(),
                CreatedDate = currentUser.CreatedDate,
                UpdatedDate = currentUser.UpdatedDate,
            };

            return new BaseResponse<DetailUserDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.retrieve", new List<string> { _localizer.Translate("user") }),
                Data = currentUserDTO
            };
        }
    }
}
