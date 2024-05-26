using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Blogs.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Helpers;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Blogs.Commands
{
    public class BlogUpdateCommandDTO
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string Content { get; init; }
    }

    public class BlogUpdateCommandDTOValidator : AbstractValidator<BlogUpdateCommandDTO>
    {
        private readonly IStringLocalizer<BlogUpdateCommandDTOValidator> _localizer;

        public BlogUpdateCommandDTOValidator(IStringLocalizer<BlogUpdateCommandDTOValidator> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Title).NotEmpty()
                                 .OverridePropertyName(_localizer.Translate("title"))
                                 .WithMessage(_localizer.Translate("cant_be_empty"));

            RuleFor(x => x.Content).NotEmpty()
                                   .OverridePropertyName(_localizer.Translate("content"))
                                   .WithMessage(_localizer.Translate("cant_be_empty"));
        }
    }

    public class BlogUpdateCommand : IRequestWithBaseResponse<MinimalBlogDTO>
    {
        public string Slug { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Content { get; init; }
    }

    internal class BlogUpdateCommandHandler : IRequestWithBaseResponseHandler<BlogUpdateCommand, MinimalBlogDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<BlogDeleteCommandHandler> _localizer;

        public BlogUpdateCommandHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<BlogDeleteCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }

        public async Task<BaseResponse<MinimalBlogDTO>> Handle(BlogUpdateCommand request, CancellationToken cancellationToken)
        {
            var existingBlog = await _dbContext.Blogs.Include(x => x.FavoriteBlogs)
                                                     .Include(x => x.BlogTags)
                                                        .ThenInclude(y => y.Tag)
                                                     .FirstOrDefaultAsync(x => x.Slug == request.Slug && x.AuthorId == _currentUser.Id, cancellationToken);

            if (existingBlog is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            // Prevent re-generated slug if title stay the same
            var newSlug = StringHelper.GenerateSlug(request.Title);

            if (!StringHelper.IsSlugContainFullname(newSlug, existingBlog.Slug))
            {
                existingBlog.Slug = newSlug;
            }

            existingBlog.Title = request.Title;
            existingBlog.Description = request.Description;
            existingBlog.Content = request.Content;

            _dbContext.Blogs.Update(existingBlog);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var blogDTO = new MinimalBlogDTO
            {
                Slug = existingBlog.Slug,
                Title = existingBlog.Title,
                Description = existingBlog.Description,
                Content = existingBlog.Content,
                TagList = existingBlog.BlogTags.Select(x => x.Tag.Name).ToList(),
                IsFavorited = existingBlog.FavoriteBlogs.Any(x => x.UserId == _currentUser.Id),
                FavoritesCount = existingBlog.FavoriteBlogs.Count(),
                CreatedDate = existingBlog.CreatedDate,
                UpdatedDate = existingBlog.UpdatedDate,
            };

            return new BaseResponse<MinimalBlogDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.update", new List<string> { "blog" }),
                Data = blogDTO
            };
        }
    }
}
