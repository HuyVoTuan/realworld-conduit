using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.LINQ;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Queries
{
    public class GetPagingUsersQuery : PagingRequestDTO, IRequestWithBaseResponse<PagingResponseDTO<MinimalUserDTO>>
    {
    }

    // Command validation
    public class GetPagingUsersQueryValidator : AbstractValidator<GetPagingUsersQuery>
    {
        private readonly IStringLocalizer<GetPagingUsersQueryValidator> _localizer;


        public GetPagingUsersQueryValidator(IStringLocalizer<GetPagingUsersQueryValidator> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.PageIndex).Must(x => int.TryParse(x, out var result) && result > 0)
                                     .OverridePropertyName(_localizer.Translate("page_index"))
                                     .WithMessage(_localizer.Translate("invalid"));

            RuleFor(x => x.PageLimit).Must(x => int.TryParse(x, out var result) && result > 0)
                                     .OverridePropertyName(_localizer.Translate("page_limit"))
                                     .WithMessage(_localizer.Translate("invalid"));
        }
    }

    internal class GetPagingUsersQuerytHandler : IRequestWithBaseResponseHandler<GetPagingUsersQuery, PagingResponseDTO<MinimalUserDTO>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<GetPagingUsersQuerytHandler> _localizer;

        public GetPagingUsersQuerytHandler(ApplicationDbContext dbContext, IStringLocalizer<GetPagingUsersQuerytHandler> localizer, ICurrentUserService currentUser)
        {
            _localizer = localizer;
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<PagingResponseDTO<MinimalUserDTO>>> Handle(GetPagingUsersQuery request, CancellationToken cancellationToken)
        {
            var usersQueryDTO = _dbContext.Users.AsNoTracking()
                                                .Select(x => new MinimalUserDTO
                                                {
                                                    Slug = x.Slug,
                                                    Username = x.Username,
                                                    Email = x.Email,
                                                    AvatarUrl = x.AvatarUrl,
                                                    Bio = x.Bio,
                                                    IsFollowing = x.UsersBeingFollowed.Any(y => y.UserThatFollowId == _currentUser.Id),
                                                    CreatedDate = x.CreatedDate,
                                                    UpdatedDate = x.UpdatedDate,
                                                })
                                                .OrderByDescending(x => x.CreatedDate);

            // Calculate item length
            var totalUsers = await usersQueryDTO.CountAsync(cancellationToken);

            // Convert string paging request fields to integer
            int.TryParse(request.PageIndex, out var pageIndex);
            int.TryParse(request.PageLimit, out var pageLimit);

            // Perform pagination on item length with request page index and page limit
            var pagedUsersDTO = await usersQueryDTO.Page(pageIndex, pageLimit)
                                                   .ToListAsync(cancellationToken);

            // Calculate total pages
            var totalPages = Math.Ceiling((double)totalUsers / pageLimit);

            return new BaseResponse<PagingResponseDTO<MinimalUserDTO>>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.retrieve", new List<string> { _localizer.Translate("users") }),
                Data = new PagingResponseDTO<MinimalUserDTO>
                {
                    PageIndex = pageIndex,
                    PageLimit = pageLimit,
                    ItemLength = totalUsers,
                    TotalPages = (int)totalPages,
                    Data = pagedUsersDTO
                }
            };
        }
    }
}

