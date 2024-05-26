using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Blogs.DTOs;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Helpers;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Blogs.Commands
{
    public class BlogCreateCommand : IRequestWithBaseResponse<MinimalBlogDTO>
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string Content { get; init; }
        public List<string> TagList { get; init; }
    };

    public class BlogCreateCommandValidator : AbstractValidator<BlogCreateCommand>
    {
        private readonly IStringLocalizer<BlogCreateCommandValidator> _localizer;

        public BlogCreateCommandValidator(IStringLocalizer<BlogCreateCommandValidator> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Title).NotEmpty()
                                 .OverridePropertyName(_localizer.Translate("title"))
                                 .WithMessage(_localizer.Translate("cant_be_empty"));

            RuleFor(x => x.Content).NotEmpty()
                                   .OverridePropertyName(_localizer.Translate("content"))
                                   .WithMessage(_localizer.Translate("cant_be_empty"));
        }

        internal class BlogCreateCommandHandler : IRequestWithBaseResponseHandler<BlogCreateCommand, MinimalBlogDTO>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly ICurrentUserService _currentUser;
            private readonly IStringLocalizer<BlogCreateCommandHandler> _localizer;

            public BlogCreateCommandHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<BlogCreateCommandHandler> localizer)
            {
                _dbContext = dbContext;
                _localizer = localizer;
                _currentUser = currentUser;
            }
            public async Task<BaseResponse<MinimalBlogDTO>> Handle(BlogCreateCommand request, CancellationToken cancellationToken)
            {
                var isBlogExist = await _dbContext.Blogs.AnyAsync(x => x.Title == request.Title, cancellationToken);

                if (isBlogExist)
                {
                    throw new RestfulAPIException(HttpStatusCode.Conflict, _localizer.Translate("existed"));
                }

                var requestFilteredTags = await RequestTagsFilter(request, cancellationToken);

                var newBlog = new Blog
                {
                    Slug = StringHelper.GenerateSlug(request.Title),
                    Title = request.Title,
                    Description = request.Description,
                    Content = request.Content,
                    AuthorId = _currentUser.Id.Value,
                    BlogTags = requestFilteredTags.Select(tag => new BlogTag { TagId = tag.Id }).ToList()
                };

                await _dbContext.Blogs.AddAsync(newBlog, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                //Add range to BlogTags based on filtered tags
                //await _dbContext.BlogTags.AddRangeAsync(requestFilteredTags.Select(tag => new BlogTag { BlogId = newBlog.Id, TagId = tag.Id }),
                //                                        cancellationToken);

                var blogDTO = new MinimalBlogDTO
                {
                    Slug = newBlog.Slug,
                    Title = newBlog.Title,
                    Description = newBlog.Description,
                    Content = newBlog.Content,
                    TagList = requestFilteredTags.Select(x => x.Name).ToList(),
                    IsFavorited = false,
                    FavoritesCount = 0,
                    CreatedDate = newBlog.CreatedDate,
                    UpdatedDate = newBlog.UpdatedDate,
                };

                return new BaseResponse<MinimalBlogDTO>
                {
                    Code = HttpStatusCode.OK,
                    Message = _localizer.Translate("successfully.register", new List<string> { $"{blogDTO.Slug}" }),
                    Data = blogDTO
                };
            }

            private async Task<List<Tag>> RequestTagsFilter(BlogCreateCommand request, CancellationToken cancellationToken)
            {
                // Step 1: Remove duplicates from the request tag list
                var processedRequestTags = request.TagList.Distinct().ToList();

                // Step 2: Check if any of the tags in the request tag list already exist in the database
                var existingTags = await _dbContext.Tags
                                                   .AsNoTracking()
                                                   .Where(x => processedRequestTags.Contains(x.Name))
                                                   .ToListAsync(cancellationToken);

                // Step 3: Determine the new tags that are not in the database
                var existingTagNames = existingTags.Select(tag => tag.Name).ToList();
                var newTagNames = processedRequestTags.Except(existingTagNames).ToList();

                var newTags = newTagNames.Select(tagName => new Tag { Name = tagName }).ToList();

                // Step 4: Add new tags to the Tag table
                if (newTags.Any())
                {
                    await _dbContext.Tags.AddRangeAsync(newTags, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                // Combine existing and new tags
                var allTags = existingTags.Concat(newTags).ToList();
                return allTags;
            }
        }
    }
}
