using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Blogs.DTOs;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.LINQ;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Blogs.Queries
{
    public class GetFriendsBlogQuery : PagingRequestDTO, IRequestWithBaseResponse<PagingResponseDTO<DetailBlogDTO>>;

    // Command validation
    public class GetFriendsBlogQueryValidator : AbstractValidator<GetFriendsBlogQuery>
    {
        private readonly IStringLocalizer<GetFriendsBlogQueryValidator> _localizer;


        public GetFriendsBlogQueryValidator(IStringLocalizer<GetFriendsBlogQueryValidator> localizer)
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

    internal class GetFriendsBlogQueryHandler : IRequestWithBaseResponseHandler<GetFriendsBlogQuery, PagingResponseDTO<DetailBlogDTO>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<GetFriendsBlogQueryHandler> _localizer;

        public GetFriendsBlogQueryHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<GetFriendsBlogQueryHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<PagingResponseDTO<DetailBlogDTO>>> Handle(GetFriendsBlogQuery request, CancellationToken cancellationToken)
        {
            var friendsBlogQueryDTO = _dbContext.Blogs.AsNoTracking()
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
                                                        IsFollowing = x.Author.UsersBeingFollowed.Any(x => x.UserThatFollowId == _currentUser.Id),
                                                        AvatarUrl = x.Author.AvatarUrl
                                                    },
                                                })
                                                .Where(x => x.Author.IsFollowing == true)
                                                .OrderByDescending(x => x.Blog.CreatedDate);

            var totalBlogs = await friendsBlogQueryDTO.CountAsync(cancellationToken);

            // Convert string paging request fields to integer
            int.TryParse(request.PageIndex, out var pageIndex);
            int.TryParse(request.PageLimit, out var pageLimit);

            var pagedFriendsBlogDTO = await friendsBlogQueryDTO.Page(pageIndex, pageLimit)
                                                               .ToListAsync(cancellationToken);

            // Calculate total pages
            var totalPages = Math.Ceiling((double)totalBlogs / pageLimit);

            return new BaseResponse<PagingResponseDTO<DetailBlogDTO>>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.retrieve", new List<string> { _localizer.Translate("friends"), _localizer.Translate("blog") }),
                Data = new PagingResponseDTO<DetailBlogDTO>
                {
                    PageIndex = pageIndex,
                    PageLimit = pageLimit,
                    ItemLength = totalBlogs,
                    TotalPages = (int)totalPages,
                    Data = pagedFriendsBlogDTO
                }
            };
        }
    }
}
