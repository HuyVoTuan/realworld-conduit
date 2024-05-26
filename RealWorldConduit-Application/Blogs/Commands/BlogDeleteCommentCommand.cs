using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Blogs.Commands
{
    public record BlogDeleteCommentCommand(string Slug, Guid Id) : IRequestWithBaseResponse;

    internal class BlogDeleteCommentCommandHandler : IRequestWithBaseResponseHandler<BlogDeleteCommentCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<BlogAddCommentCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUser;

        public BlogDeleteCommentCommandHandler(ApplicationDbContext dbContext, IStringLocalizer<BlogAddCommentCommandHandler> localizer, ICurrentUserService currentUser)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse> Handle(BlogDeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var existingComment = await _dbContext.Comments.Include(x => x.Blog)
                                                           .Include(x => x.User)
                                                           .FirstOrDefaultAsync(x => x.Id == request.Id &&
                                                                                x.Blog.Slug == request.Slug, cancellationToken);
            if (existingComment.UserId != _currentUser.Id)
            {
                throw new RestfulAPIException(HttpStatusCode.BadRequest, _localizer.Translate("cant_delete"));
            }

            if (existingComment is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            _dbContext.Comments.Remove(existingComment);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse(HttpStatusCode.NoContent);
        }
    }
}
