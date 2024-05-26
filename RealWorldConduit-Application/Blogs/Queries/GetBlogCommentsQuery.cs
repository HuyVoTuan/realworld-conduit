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
    public record GetBlogCommentsQuery(string Slug) : IRequestWithBaseResponse<DetailCommentDTO>;

    internal class GetBlogCommentsQueryHandler : IRequestWithBaseResponseHandler<GetBlogCommentsQuery, DetailCommentDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<GetBlogCommentsQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUser;

        public GetBlogCommentsQueryHandler(ApplicationDbContext dbContext, IStringLocalizer<GetBlogCommentsQueryHandler> localizer, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<DetailCommentDTO>> Handle(GetBlogCommentsQuery request, CancellationToken cancellationToken)
        {
            var existingBlog = await _dbContext.Blogs.Include(x => x.Comments)
                                                          .ThenInclude(y => y.User)
                                                          .ThenInclude(z => z.UsersBeingFollowed)
                                                     .AsSplitQuery()
                                                     .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);


            if (existingBlog == null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            var blogCommentsDTO = new DetailCommentDTO
            {
                Comments = existingBlog.Comments.Select(x => new MinimalCommentDTO
                {
                    Author = new MinimalUserDTO
                    {
                        Slug = x.User.Slug,
                        Username = x.User.Username,
                        Email = x.User.Email,
                        AvatarUrl = x.User.AvatarUrl,
                        Bio = x.User.Bio,
                        IsFollowing = x.User.UsersBeingFollowed.Any(y => y.UserThatFollowId == _currentUser.Id),
                        CreatedDate = x.User.CreatedDate,
                        UpdatedDate = x.User.UpdatedDate,
                    },
                    Content = x.Content,
                }).ToList()
            };

            return new BaseResponse<DetailCommentDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.retrieve"),
                Data = blogCommentsDTO
            };
        }
    }
}
