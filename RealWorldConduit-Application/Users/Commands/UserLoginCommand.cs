using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Commands
{
    public class UserLoginCommand : IRequestWithBaseResponse<AuthResponseDTO>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        private readonly IStringLocalizer<UserLoginCommandValidator> _localizer;

        public UserLoginCommandValidator(IStringLocalizer<UserLoginCommandValidator> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Email).NotEmpty()
                                 .OverridePropertyName(_localizer.Translate("email"))
                                 .WithMessage(_localizer.Translate("cannot_be_empty"))
                                 .EmailAddress()
                                 .OverridePropertyName(_localizer.Translate("email"))
                                 .WithMessage(_localizer.Translate("invalid"));


            RuleFor(x => x.Password).NotEmpty()
                                    .OverridePropertyName(_localizer.Translate("password"))
                                    .WithMessage(_localizer.Translate("cannot_be_empty"))
                                    .MinimumLength(6)
                                    .OverridePropertyName(_localizer.Translate("password"))
                                    .WithMessage(_localizer.Translate("invalid_length"));
        }
    }
    internal class UserLoginCommandHandler : IRequestWithBaseResponseHandler<UserLoginCommand, AuthResponseDTO>
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<UserLoginCommandHandler> _localizer;

        public UserLoginCommandHandler(
        ApplicationDbContext dbContext,
        IAuthService authService,
        IStringLocalizer<UserLoginCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _authService = authService;
        }
        public async Task<BaseResponseDTO<AuthResponseDTO>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users
                                              .AsNoTracking()
                                              .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (existingUser is null || !_authService.VerifyPassword(request.Password, existingUser.Password))
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            var newRefreshToken = _authService.GenerateRefreshToken(existingUser);
            await _dbContext.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BaseResponseDTO<AuthResponseDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successfully.login"),
                Data = new AuthResponseDTO
                {
                    AccessToken = _authService.GenerateToken(existingUser),
                    RefreshToken = newRefreshToken.RefreshTokenString,
                }
            };
        }
    }
}
