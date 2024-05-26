using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using RealWorldConduit_Infrastructure.Services.Cache;
using System.Net;

namespace RealWorldConduit_Application.Users.Commands
{
    public record UserRefreshTokenCommand(string RefreshToken) : IRequestWithBaseResponse<AuthResponseDTO>;

    public class UserRefreshTokenCommandValidator : AbstractValidator<UserRefreshTokenCommand>
    {
        private readonly IStringLocalizer<UserRefreshTokenCommandValidator> _localizer;

        public UserRefreshTokenCommandValidator(IStringLocalizer<UserRefreshTokenCommandValidator> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.RefreshToken).NotEmpty()
                                        .OverridePropertyName(_localizer.Translate("refresh-token"))
                                        .WithMessage(_localizer.Translate("cannot_be_empty"));
        }
    }

    internal class UserRefreshTokenCommandHandler : IRequestWithBaseResponseHandler<UserRefreshTokenCommand, AuthResponseDTO>
    {
        private readonly IAuthService _authService;
        private readonly ICacheService _cacheService;
        private readonly ICurrentUserService _currentUser;
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<UserRefreshTokenCommandHandler> _localizer;

        public UserRefreshTokenCommandHandler(
        IAuthService authService,
        ICacheService cacheService,
        ApplicationDbContext dbContext,
        ICurrentUserService currentUser,
        IStringLocalizer<UserRefreshTokenCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _authService = authService;
            _currentUser = currentUser;
            _cacheService = cacheService;
            _localizer = localizer;
        }
        public async Task<BaseResponse<AuthResponseDTO>> Handle(UserRefreshTokenCommand request, CancellationToken cancellationToken)
        {

            var oldRefreshToken = await _dbContext.RefreshTokens.Include(x => x.User)
                                                                .FirstOrDefaultAsync(x => x.RefreshTokenString == request.RefreshToken
                                                                                     && x.ExpiredTime > DateTime.UtcNow
                                                                                     && x.UserId == _currentUser.Id
                                                                                     && x.User.isActive == true,
                                                                                     cancellationToken);


            if (oldRefreshToken is null)
            {
                // Check if previous refresh token is still in the cache.
                return CachedRefreshTokenHandler(request.RefreshToken);
            }

            _dbContext.RefreshTokens.Remove(oldRefreshToken);

            var newAccessToken = _authService.GenerateToken(oldRefreshToken.User);
            var newRefreshToken = _authService.GenerateRefreshToken(oldRefreshToken.User);

            await _dbContext.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);

            var authDTO = new AuthResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.RefreshTokenString
            };

            // Set the old refresh token with new authDTO response in the cache
            _cacheService.SetData<AuthResponseDTO>($"refreshTokenResponse-{oldRefreshToken.RefreshTokenString}", authDTO, TimeSpan.FromSeconds(20));

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponse<AuthResponseDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successfully.refresh-user-token"),
                Data = authDTO
            };
        }

        private BaseResponse<AuthResponseDTO> CachedRefreshTokenHandler(string refreshToken)
        {
            var cachedRefreshToken = _cacheService.GetData<AuthResponseDTO>($"refreshTokenResponse-{refreshToken}");

            if (cachedRefreshToken is null)
            {
                throw new RestfulAPIException(HttpStatusCode.Unauthorized, _localizer.Translate("unauthorized"));
            }

            return new BaseResponse<AuthResponseDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successfully.retrieve"),
                Data = cachedRefreshToken
            };
        }
    }
}
