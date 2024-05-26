using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWorldConduit_Application.Users.Commands;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Application.Users.Queries;
using RealWorldConduit_Infrastructure.Commons.Base;

namespace RealWorldConduit_API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPagingUsers([FromQuery] GetPagingUsersQuery request, CancellationToken cancellationToken)
        {
            var getPagingUsersResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<PagingResponseDTO<MinimalUserDTO>>>
            {
                StatusCode = getPagingUsersResult.Code,
                Data = getPagingUsersResult,
            };
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetSingleUser([FromRoute] GetSingleUserQuery request, CancellationToken cancellationToken)
        {
            var getSingleUserResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<MinimalUserDTO>>
            {
                StatusCode = getSingleUserResult.Code,
                Data = getSingleUserResult,
            };
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var userRegisterResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<AuthResponseDTO>>
            {
                StatusCode = userRegisterResult.Code,
                Data = userRegisterResult,
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginCommand request, CancellationToken cancellationToken)
        {
            var userLoginResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<AuthResponseDTO>>
            {
                StatusCode = userLoginResult.Code,
                Data = userLoginResult,
            };
        }

        [Authorize]
        [HttpPost("/api/users/{slug}/follow")]
        public async Task<IActionResult> UserFollow([FromRoute] UpsertUserFollowCommand request, CancellationToken cancellationToken)
        {
            var userFollowResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<DetailUserDTO>>
            {
                StatusCode = userFollowResult.Code,
                Data = userFollowResult
            };
        }

        [Authorize]
        [HttpGet("/api/user")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var request = new GetCurrentUserQuery();
            var getCurrentUserResult = await _mediator.Send(request, cancellationToken);

            return new CustomActionResult<BaseResponse<DetailUserDTO>>
            {
                StatusCode = getCurrentUserResult.Code,
                Data = getCurrentUserResult,
            };
        }



        [Authorize]
        [HttpPost("/api/user/refresh-token")]
        public async Task<IActionResult> UserRefreshToken([FromBody] UserRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userRefreshTokenResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<AuthResponseDTO>>
            {
                StatusCode = userRefreshTokenResult.Code,
                Data = userRefreshTokenResult,
            };
        }

        [Authorize]
        [HttpDelete("/api/user/revoke-token")]
        public async Task<IActionResult> UserRevokeToken(CancellationToken cancellationToken)
        {
            var request = new UserRevokeTokenCommand();
            var userRevokeTokenResult = await _mediator.Send(request, cancellationToken);

            return new CustomActionResult<BaseResponse>
            {
                StatusCode = userRevokeTokenResult.Code,
                Data = userRevokeTokenResult,
            };
        }

        [Authorize]
        [HttpPut("/api/user")]
        public async Task<IActionResult> UpdateUserDetail([FromBody] UserUpdateDetailCommand request, CancellationToken cancellationToken)
        {
            var updateUserDetailResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse<MinimalUserDTO>>
            {
                StatusCode = updateUserDetailResult.Code,
                Data = updateUserDetailResult
            };
        }

        [Authorize]
        [HttpPut("/api/user/deactivate")]
        public async Task<IActionResult> UserDeactivate([FromRoute] UserDeactivateCommand request, CancellationToken cancellationToken)
        {
            var userDeactivationResult = await _mediator.Send(request, cancellationToken);
            return new CustomActionResult<BaseResponse>
            {
                StatusCode = userDeactivationResult.Code,
                Data = userDeactivationResult
            };
        }
    }
}
