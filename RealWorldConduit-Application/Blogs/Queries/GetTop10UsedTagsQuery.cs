using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Blogs.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using System.Net;

namespace RealWorldConduit_Application.Blogs.Queries
{
    public record GetTop10UsedTagsQuery() : IRequestWithBaseResponse<TagListDTO>;

    internal class GetTop10UsedTagsQueryHandler : IRequestWithBaseResponseHandler<GetTop10UsedTagsQuery, TagListDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<GetTop10UsedTagsQueryHandler> _localizer;

        public GetTop10UsedTagsQueryHandler(ApplicationDbContext dbContext, IStringLocalizer<GetTop10UsedTagsQueryHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
        }

        public async Task<BaseResponse<TagListDTO>> Handle(GetTop10UsedTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _dbContext.BlogTags.AsNoTracking()
                                                .GroupBy(x => x.Tag.Name)
                                                .OrderByDescending(group => group.Count())
                                                .Take(10)
                                                .Select(x => x.Key)
                                                .ToListAsync(cancellationToken);

            var tagsDTO = new TagListDTO
            {
                Tags = tags
            };

            return new BaseResponse<TagListDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.retrieve", new List<string> { _localizer.Translate("tags") }),
                Data = tagsDTO
            };
        }
    }
}
