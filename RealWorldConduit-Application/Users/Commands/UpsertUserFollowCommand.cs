using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Commands
{
    public record UpsertUserFollowCommand(string Slug) : IRequestWithBaseResponse<DetailUserDTO>;

    internal class UserFollowCommandHandler : IRequestWithBaseResponseHandler<UpsertUserFollowCommand, DetailUserDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<UpsertUserFollowCommand> _localizer;

        public UserFollowCommandHandler(ApplicationDbContext dbContext, IStringLocalizer<UpsertUserFollowCommand> localizer, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<DetailUserDTO>> Handle(UpsertUserFollowCommand request, CancellationToken cancellationToken)
        {
            int flag = 0;
            var userToFollowOrUnfollow = await _dbContext.Users.AsNoTracking()
                                                     .Include(x => x.UsersBeingFollowed)
                                                     .FirstOrDefaultAsync(x => x.Slug == request.Slug &&
                                                                          x.isActive == true, cancellationToken);

            if (userToFollowOrUnfollow is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            if (userToFollowOrUnfollow.Slug == _currentUser.Slug)
            {
                throw new RestfulAPIException(HttpStatusCode.BadRequest, _localizer.Translate("invalid_self_following"));
            }

            // Get the existing friendship between the users
            var existingFriendship = userToFollowOrUnfollow.UsersBeingFollowed.FirstOrDefault(x => x.UserThatFollowId == _currentUser.Id);

            if (existingFriendship is null)
            {
                flag = 1;
                var newFriendship = new Friendship
                {
                    UserThatFollowId = (Guid)_currentUser.Id,
                    UserBeingFollowedId = userToFollowOrUnfollow.Id,
                };

                await _dbContext.Friendships.AddAsync(newFriendship, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                _dbContext.Friendships.Remove(existingFriendship);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            DetailUserDTO userWithFriendshipDTO = await MappingToDetailUserDTO(cancellationToken);

            return new BaseResponse<DetailUserDTO>
            {
                Code = HttpStatusCode.OK,
                Message = flag == 1 ? _localizer.Translate("successful.follow") : _localizer.Translate("successful.unfollow"),
                Data = userWithFriendshipDTO,
            };
        }

        private async Task<DetailUserDTO> MappingToDetailUserDTO(CancellationToken cancellationToken)
        {
            var currentUser = await _dbContext.Users.AsNoTracking()
                                                    .Include(x => x.UsersThatFollow)
                                                        .ThenInclude(y => y.UserBeingFollowed)
                                                    .FirstOrDefaultAsync(x => x.Id == _currentUser.Id, cancellationToken);

            var userWithFriendshipDTO = new DetailUserDTO
            {
                Slug = currentUser.Slug,
                Username = currentUser.Username,
                AvatarUrl = currentUser.AvatarUrl,
                Email = currentUser.Email,
                Bio = currentUser.Bio,
                Friends = currentUser.UsersThatFollow.Select(x => new MinimalUserDTO
                {
                    Slug = x.UserBeingFollowed.Slug,
                    Username = x.UserBeingFollowed.Username,
                    AvatarUrl = x.UserBeingFollowed.AvatarUrl,
                    Email = x.UserBeingFollowed.Email,
                    Bio = x.UserBeingFollowed.Bio,
                    IsFollowing = true,
                    CreatedDate = x.UserBeingFollowed.CreatedDate,
                    UpdatedDate = x.UserBeingFollowed.UpdatedDate
                }).ToList(),
                CreatedDate = currentUser.CreatedDate,
                UpdatedDate = currentUser.UpdatedDate,
            };
            return userWithFriendshipDTO;
        }
    }
}
