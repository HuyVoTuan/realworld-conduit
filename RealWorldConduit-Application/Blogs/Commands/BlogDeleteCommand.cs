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
    public record BlogDeleteCommand(string Slug) : IRequestWithBaseResponse;

    internal class BlogDeleteCommandHandler : IRequestWithBaseResponseHandler<BlogDeleteCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUser;
        private readonly IStringLocalizer<BlogDeleteCommandHandler> _localizer;

        public BlogDeleteCommandHandler(ApplicationDbContext dbContext, ICurrentUserService currentUser, IStringLocalizer<BlogDeleteCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse> Handle(BlogDeleteCommand request, CancellationToken cancellationToken)
        {
            var existingBlog = await _dbContext.Blogs.FirstOrDefaultAsync(x => x.Slug == request.Slug && x.AuthorId == _currentUser.Id, cancellationToken);

            if (existingBlog is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            _dbContext.Blogs.Remove(existingBlog);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse(HttpStatusCode.NoContent);
        }
    }
}
