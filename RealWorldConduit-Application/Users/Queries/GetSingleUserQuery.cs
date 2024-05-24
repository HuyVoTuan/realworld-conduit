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
    public record GetSingleUserQuery(string Slug) : IRequestWithBaseResponse<MinimalUserDTO>;

    internal class GetSingleUserQueryHandler : IRequestWithBaseResponseHandler<GetSingleUserQuery, MinimalUserDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<GetSingleUserQueryHandler> _localizer;

        public GetSingleUserQueryHandler(ApplicationDbContext dbContext, IStringLocalizer<GetSingleUserQueryHandler> localizer)
        {
            _localizer = localizer;
            _dbContext = dbContext;
        }
        public async Task<BaseResponse<MinimalUserDTO>> Handle(GetSingleUserQuery request, CancellationToken cancellationToken)
        {
            var userDTOQuery = await _dbContext.Users.AsNoTracking()
                                                     .Select(x => new MinimalUserDTO
                                                     {
                                                         Slug = x.Slug,
                                                         Username = x.Username,
                                                         Email = x.Email,
                                                         AvatarUrl = x.AvatarUrl,
                                                         Bio = x.Bio,
                                                         CreatedDate = x.CreatedDate,
                                                         UpdatedDate = x.UpdatedDate,
                                                     })
                                                     .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);


            if (userDTOQuery is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found", new List<string> { _localizer.Translate("user") }));
            }

            return new BaseResponse<MinimalUserDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("succesful.retrieve", new List<string> { _localizer.Translate("user") }),
                Data = userDTOQuery
            };
        }
    }
}
