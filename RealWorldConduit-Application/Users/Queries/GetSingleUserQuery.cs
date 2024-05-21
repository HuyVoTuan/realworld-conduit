using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using System.Net;

namespace RealWorldConduit_Application.Users.Queries
{
    public record GetSingleUserQuery(string Slug) : IRequestWithBaseResponse<UserDTO>;

    internal class GetSingleUserQueryHandler : IRequestWithBaseResponseHandler<GetSingleUserQuery, UserDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<GetSingleUserQueryHandler> _localizer;

        public GetSingleUserQueryHandler(ApplicationDbContext dbContext, IStringLocalizer<GetSingleUserQueryHandler> localizer)
        {
            _localizer = localizer;
            _dbContext = dbContext;
        }
        public async Task<BaseResponse<UserDTO>> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
        {
            var userDTOQuery = await _dbContext.Users.AsNoTracking()
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
                                                     .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);


            if (userDTOQuery is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found", new List<string> { _localizer.Translate("user") }));
            }

            return new BaseResponse<UserDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("succesful.retrieve", new List<string> { _localizer.Translate("user") }),
                Data = userDTOQuery
            };
        }
    }
}
