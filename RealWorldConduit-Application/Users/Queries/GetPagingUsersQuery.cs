using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.LINQ;
using System.Net;

namespace RealWorldConduit_Application.Users.Queries
{
    public class GetPagingUsersQuery : PagingRequestDTO, IRequestWithBaseResponse<PagingResponseDTO<UserDTO>>
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

        internal class GetPagingUsersQuerytHandler : IRequestWithBaseResponseHandler<GetPagingUsersQuery, PagingResponseDTO<UserDTO>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IStringLocalizer<GetPagingUsersQuerytHandler> _localizer;

            public GetPagingUsersQuerytHandler(ApplicationDbContext dbContext, IStringLocalizer<GetPagingUsersQuerytHandler> localizer)
            {
                _localizer = localizer;
                _dbContext = dbContext;
            }
            public async Task<BaseResponse<PagingResponseDTO<UserDTO>>> Handle(GetPagingUsersQuery request, CancellationToken cancellationToken)
            {
                var query = _dbContext.Users.AsNoTracking()
                                            .Select(x => new UserDTO
                                            {
                                                Slug = x.Slug,
                                                Username = x.Username,
                                                Email = x.Email,
                                                AvatarUrl = x.AvatarUrl,
                                                Bio = x.Bio,
                                                CreatedDate = x.CreatedDate,
                                                UpdatedDate = x.UpdatedDate,
                                                Locations = x.Locations.Select(l => new LocationDTO
                                                {
                                                    Address = l.Address,
                                                    District = l.District,
                                                    Ward = l.Ward,
                                                    City = l.City
                                                })
                                            })
                                            .OrderByDescending(x => x.CreatedDate);

                // Calculate item length
                var totalUsers = await query.CountAsync(cancellationToken);

                // Convert string paging request fields to integer
                int.TryParse(request.PageIndex, out var pageIndex);
                int.TryParse(request.PageLimit, out var pageLimit);

                // Perform pagination on item length with request page index and page limit
                var pagedUsersDTO = await query.Page(pageIndex, pageLimit)
                                               .ToListAsync(cancellationToken);

                // Calculate total pages
                var totalPages = Math.Ceiling((double)totalUsers / pageLimit);

                return new BaseResponse<PagingResponseDTO<UserDTO>>
                {
                    Code = HttpStatusCode.OK,
                    Message = _localizer.Translate("successful.retrieve", new List<string> { _localizer.Translate("users") }),
                    Data = new PagingResponseDTO<UserDTO>
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
}
