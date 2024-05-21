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
    public record GetCurrentUserQuery() : IRequestWithBaseResponse<UserDTO>;
    internal class GetCurrentUserQueryHandler : IRequestWithBaseResponseHandler<GetCurrentUserQuery, UserDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<GetCurrentUserQueryHandler> _localizer;

        public GetCurrentUserQueryHandler(ApplicationDbContext dbContext, ICurrentUserService currentUserService, IStringLocalizer<GetCurrentUserQueryHandler> localizer)
        {
            _localizer = localizer;
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        public async Task<BaseResponse<UserDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _dbContext.Users.AsNoTracking()
                                                    .Include(x => x.Locations)
                                                    .FirstOrDefaultAsync(x => x.Id == _currentUserService.Id, cancellationToken);

            var currentUserDTO = new UserDTO
            {
                Slug = currentUser.Slug,
                Username = currentUser.Username,
                AvatarUrl = currentUser.AvatarUrl,
                Bio = currentUser.Bio,
                Email = currentUser.Email,
                CreatedDate = currentUser.CreatedDate,
                UpdatedDate = currentUser.UpdatedDate,
                Locations = currentUser.Locations.Select(x => new LocationDTO
                {
                    Slug = x.Slug,
                    Address = x.Address,
                    District = x.District,
                    City = x.City,
                })
            };

            return new BaseResponse<UserDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.retrieve", new List<string> { _localizer.Translate("user") }),
                Data = currentUserDTO
            };
        }
    }
}
