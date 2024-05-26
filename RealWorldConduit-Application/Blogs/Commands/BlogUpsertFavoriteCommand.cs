using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Blogs.DTOs;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Blogs.Commands
{
    public record BlogUpsertFavoriteCommand(string Slug) : IRequestWithBaseResponse<MinimalBlogDTO>;

    internal class BlogUpsertFavoriteCommandHandler : IRequestWithBaseResponseHandler<BlogUpsertFavoriteCommand, MinimalBlogDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<BlogUpsertFavoriteCommandHandler> _localizer;

        public BlogUpsertFavoriteCommandHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<BlogUpsertFavoriteCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<MinimalBlogDTO>> Handle(BlogUpsertFavoriteCommand request, CancellationToken cancellationToken)
        {
            int flag = 0;
            var blogToFavorite = await _dbContext.Blogs
                                                .Include(x => x.FavoriteBlogs)
                                                .Include(x => x.BlogTags)
                                                    .ThenInclude(y => y.Tag)
                                                .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

            if (blogToFavorite is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            if (!blogToFavorite.FavoriteBlogs.Any(x => x.UserId == _currentUser.Id))
            {
                flag = 1;
                var newFavoriteBlog = new FavoriteBlog
                {
                    BlogId = blogToFavorite.Id,
                    UserId = (Guid)_currentUser.Id
                };

                await _dbContext.FavoriteBlogs.AddAsync(newFavoriteBlog, cancellationToken);
            }
            else
            {
                _dbContext.FavoriteBlogs.Remove(blogToFavorite.FavoriteBlogs.FirstOrDefault(x => x.UserId == _currentUser.Id));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            var blogDTO = new MinimalBlogDTO
            {
                Slug = blogToFavorite.Slug,
                Title = blogToFavorite.Title,
                Description = blogToFavorite.Description,
                Content = blogToFavorite.Content,
                TagList = blogToFavorite.BlogTags.Select(x => x.Tag.Name).ToList(),
                IsFavorited = blogToFavorite.FavoriteBlogs.Any(x => x.UserId == _currentUser.Id),
                FavoritesCount = blogToFavorite.FavoriteBlogs.Count(),
                CreatedDate = blogToFavorite.CreatedDate,
                UpdatedDate = blogToFavorite.UpdatedDate,
            };

            return new BaseResponse<MinimalBlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = flag == 1 ?
                          _localizer.Translate("successfully.favorite", new List<string> { _localizer.Translate("blog") }) :
                          _localizer.Translate("successfully.unfavorite", new List<string> { _localizer.Translate("blog") }),
                Data = blogDTO
            };
        }
    }
}
