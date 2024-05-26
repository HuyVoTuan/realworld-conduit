using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWorldConduit_Application.Blogs.Commands;
using RealWorldConduit_Application.Blogs.DTOs;
using RealWorldConduit_Application.Blogs.Queries;
using RealWorldConduit_Infrastructure.Commons.Base;

namespace RealWorldConduit_API.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/tags")]
        public async Task<IActionResult> GetTop10UsedTags([FromQuery] GetTop10UsedTagsQuery request, CancellationToken cancellationToken)
        {
            var getTop10UsedTagsResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<TagListDTO>>
            {
                StatusCode = getTop10UsedTagsResult.Code,
                Data = getTop10UsedTagsResult,
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetGlobalBlogs([FromQuery] GetPagingBlogsQuery request, CancellationToken cancellationToken)
        {
            var getPagingBlogsResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<PagingResponseDTO<DetailBlogDTO>>>
            {
                StatusCode = getPagingBlogsResult.Code,
                Data = getPagingBlogsResult,
            };
        }

        [Authorize]
        [HttpGet("feed")]
        public async Task<IActionResult> GetFriendsBlog([FromQuery] GetFriendsBlogQuery request, CancellationToken cancellationToken)
        {
            var getFriendsBlogResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<PagingResponseDTO<DetailBlogDTO>>>
            {
                StatusCode = getFriendsBlogResult.Code,
                Data = getFriendsBlogResult,
            };
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetSingleBlog([FromRoute] GetSingleBlogQuery request, CancellationToken cancellationToken)
        {
            var getSingleBlogResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<DetailBlogDTO>>
            {
                StatusCode = getSingleBlogResult.Code,
                Data = getSingleBlogResult,
            };
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> BlogCreate([FromBody] BlogCreateCommand request, CancellationToken cancellationToken)
        {
            var blogCreateResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<MinimalBlogDTO>>
            {
                StatusCode = blogCreateResult.Code,
                Data = blogCreateResult,
            };
        }

        [Authorize]
        [HttpPut("{slug}")]
        public async Task<IActionResult> BlogUpdate([FromRoute] string slug, [FromBody] BlogUpdateCommandDTO requestDTO, CancellationToken cancellationToken)
        {
            var request = new BlogUpdateCommand
            {
                Slug = slug,
                Title = requestDTO.Title,
                Content = requestDTO.Content,
                Description = requestDTO.Description
            };

            var blogUpdateResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<MinimalBlogDTO>>
            {
                StatusCode = blogUpdateResult.Code,
                Data = blogUpdateResult,
            };
        }

        [Authorize]
        [HttpDelete("{slug}")]
        public async Task<IActionResult> BlogDelete([FromRoute] BlogDeleteCommand request, CancellationToken cancellationToken)
        {
            var blogDeleteResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse>
            {
                StatusCode = blogDeleteResult.Code,
                Data = blogDeleteResult,
            };
        }

        [Authorize]
        [HttpPost("{slug}/favorite")]
        public async Task<IActionResult> BlogUpsertFavorite([FromRoute] BlogUpsertFavoriteCommand request, CancellationToken cancellationToken)
        {
            var blogUpsertFavoriteResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<MinimalBlogDTO>>
            {
                StatusCode = blogUpsertFavoriteResult.Code,
                Data = blogUpsertFavoriteResult,
            };
        }

        [Authorize]
        [HttpGet("{slug}/comments")]
        public async Task<IActionResult> BlogGetComments([FromRoute] GetBlogCommentsQuery request, CancellationToken cancellationToken)
        {
            var blogGetCommentsResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<DetailCommentDTO>>
            {
                StatusCode = blogGetCommentsResult.Code,
                Data = blogGetCommentsResult,
            };
        }

        [Authorize]
        [HttpPost("{slug}/comments")]
        public async Task<IActionResult> BlogAddComment([FromRoute] string slug, [FromBody] BlogAddCommentCommandDTO requestDTO, CancellationToken cancellationToken)
        {
            var request = new BlogAddCommentCommand(slug, requestDTO.Content);
            var blogAddCommentResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<MinimalCommentDTO>>
            {
                StatusCode = blogAddCommentResult.Code,
                Data = blogAddCommentResult,
            };
        }

        [Authorize]
        [HttpDelete("{slug}/comments/{id}")]
        public async Task<IActionResult> BlogDeleteComment([FromRoute] BlogDeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var blogDeleteCommentResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse>
            {
                StatusCode = blogDeleteCommentResult.Code,
                Data = blogDeleteCommentResult,
            };
        }
    }
}
