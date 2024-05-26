using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Blogs.DTOs;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Blogs.Commands
{
    public record BlogAddCommentCommandDTO(string Content);

    public class BlogAddCommentCommandValidator : AbstractValidator<BlogAddCommentCommandDTO>
    {
        private readonly IStringLocalizer<BlogAddCommentCommandValidator> _localizer;

        public BlogAddCommentCommandValidator(IStringLocalizer<BlogAddCommentCommandValidator> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Content).NotEmpty()
                                   .OverridePropertyName(_localizer.Translate("content"))
                                   .WithMessage(_localizer.Translate("cant_be_empty"));
        }
    }

    public record BlogAddCommentCommand(string Slug, string Content) : IRequestWithBaseResponse<MinimalCommentDTO>;

    internal class BlogAddCommentCommandHandler : IRequestWithBaseResponseHandler<BlogAddCommentCommand, MinimalCommentDTO>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<BlogAddCommentCommandHandler> _localizer;

        public BlogAddCommentCommandHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<BlogAddCommentCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
            _localizer = localizer;
        }
        public async Task<BaseResponse<MinimalCommentDTO>> Handle(BlogAddCommentCommand request, CancellationToken cancellationToken)
        {
            var blogToAddComment = await _dbContext.Blogs.Include(x => x.Author)
                                                            .ThenInclude(x => x.UsersBeingFollowed)
                                                         .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

            if (blogToAddComment is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            var newComment = new Comment
            {
                BlogId = blogToAddComment.Id,
                UserId = (Guid)_currentUser.Id,
                Content = request.Content,
            };

            await _dbContext.Comments.AddAsync(newComment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var newCommentDTO = new MinimalCommentDTO
            {
                Content = newComment.Content,
                Author = new MinimalUserDTO
                {
                    Slug = blogToAddComment.Author.Slug,
                    Username = blogToAddComment.Author.Username,
                    Email = blogToAddComment.Author.Email,
                    AvatarUrl = blogToAddComment.Author.AvatarUrl,
                    Bio = blogToAddComment.Author.Bio,
                    IsFollowing = blogToAddComment.Author.UsersBeingFollowed.Any(x => x.UserThatFollowId == _currentUser.Id),
                    CreatedDate = blogToAddComment.Author.CreatedDate,
                    UpdatedDate = blogToAddComment.Author.UpdatedDate,
                },
                CreatedDate = newComment.CreatedDate,
                UpdatedDate = newComment.UpdatedDate,
            };

            return new BaseResponse<MinimalCommentDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.comment"),
                Data = newCommentDTO
            };
        }
    }
}
