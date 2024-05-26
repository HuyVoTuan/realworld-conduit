using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Blogs.DTOs;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Blogs.Queries
{
    public record GetSingleBlogQuery(string Slug) : IRequestWithBaseResponse<DetailBlogDTO>;

    internal class GetABlogQueryHandler : IRequestWithBaseResponseHandler<GetSingleBlogQuery, DetailBlogDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<GetABlogQueryHandler> _localizer;

        public GetABlogQueryHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<GetABlogQueryHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<DetailBlogDTO>> Handle(GetSingleBlogQuery request, CancellationToken cancellationToken)
        {
            var blogDTO = await _dbContext.Blogs.AsNoTracking()
                                                .Select(x => new DetailBlogDTO
                                                {
                                                    Blog = new MinimalBlogDTO
                                                    {
                                                        Slug = x.Slug,
                                                        Title = x.Title,
                                                        Description = x.Description,
                                                        Content = x.Content,
                                                        TagList = x.BlogTags.Select(x => x.Tag.Name).ToList(),
                                                        IsFavorited = x.FavoriteBlogs.Any(x => x.UserId == _currentUser.Id),
                                                        FavoritesCount = x.FavoriteBlogs.Count(),
                                                        CreatedDate = x.CreatedDate,
                                                        UpdatedDate = x.UpdatedDate,
                                                    },
                                                    Author = new MinimalUserDTO
                                                    {
                                                        Slug = x.Author.Slug,
                                                        Username = x.Author.Username,
                                                        Email = x.Author.Email,
                                                        Bio = x.Author.Bio,
                                                        IsFollowing = x.Author.UsersBeingFollowed.Any(x => x.UserBeingFollowedId == _currentUser.Id),
                                                        AvatarUrl = x.Author.AvatarUrl
                                                    },
                                                })
                                                .FirstOrDefaultAsync(x => x.Blog.Slug == request.Slug, cancellationToken);


            if (blogDTO is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            return new BaseResponse<DetailBlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.retrieve", new List<string> { _localizer.Translate("blog") }),
                Data = blogDTO
            };

        }
    }
}
