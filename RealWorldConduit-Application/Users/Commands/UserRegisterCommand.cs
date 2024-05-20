using FluentValidation;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Helpers;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Commands
{
    public class UserRegisterCommand : IRequestWithBaseResponse<AuthResponseDTO>
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string Address { get; init; }
        public string District { get; init; }
        public string Ward { get; init; }
        public string City { get; init; }
    }

    public class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<UserRegisterCommandValidator> _localizer;

        public UserRegisterCommandValidator(ApplicationDbContext dbContext, IStringLocalizer<UserRegisterCommandValidator> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;

            RuleFor(x => x.Username).NotEmpty()
                                    .OverridePropertyName(_localizer.Translate("username"))
                                    .WithMessage(_localizer.Translate("cannot_be_empty"));


            RuleFor(x => x.Email).NotEmpty()
                                 .OverridePropertyName(_localizer.Translate("email"))
                                 .WithMessage(_localizer.Translate("cannot_be_empty"))
                                 .EmailAddress()
                                 .OverridePropertyName(_localizer.Translate("email"))
                                 .WithMessage(_localizer.Translate("invalid"))
                                 .Must(email =>
                                 {
                                     var isExisted = _dbContext.Users.Any(u => u.Email == email);
                                     return !isExisted;
                                 })
                                 .OverridePropertyName(_localizer.Translate("email"))
                                 .WithMessage(_localizer.Translate("existed"));


            RuleFor(x => x.Password).NotEmpty()
                                    .OverridePropertyName(_localizer.Translate("password"))
                                    .WithMessage(_localizer.Translate("cannot_be_empty"))
                                    .MinimumLength(6)
                                    .OverridePropertyName(_localizer.Translate("password"))
                                    .WithMessage(_localizer.Translate("invalid_length"));

            RuleFor(x => x.Address).NotEmpty()
                                   .OverridePropertyName(_localizer.Translate("address"))
                                   .WithMessage(_localizer.Translate("cant_be_empty"));

            RuleFor(x => x.District).NotEmpty()
                                    .OverridePropertyName(_localizer.Translate("district"))
                                    .WithMessage(_localizer.Translate("cant_be_empty"));

            RuleFor(x => x.Ward).NotEmpty()
                               .OverridePropertyName(_localizer.Translate("ward"))
                               .WithMessage(_localizer.Translate("failure.cant_be_empty"));

            RuleFor(x => x.City).NotEmpty()
                                .OverridePropertyName(_localizer.Translate("city"))
                                .WithMessage(_localizer.Translate("cant_be_empty"))
                                .Must(StringHelper.IsValidString)
                                .OverridePropertyName(_localizer.Translate("city"))
                                .WithMessage(_localizer.Translate("invalid"));
        }

        internal class UserRegisterCommandHandler : IRequestWithBaseResponseHandler<UserRegisterCommand, AuthResponseDTO>
        {
            private readonly IAuthService _authService;
            private readonly ApplicationDbContext _dbContext;
            private readonly IStringLocalizer<UserRegisterCommandHandler> _localizer;

            public UserRegisterCommandHandler(
            ApplicationDbContext dbContext,
            IAuthService authService,
            IStringLocalizer<UserRegisterCommandHandler> localizer)
            {
                _dbContext = dbContext;
                _localizer = localizer;
                _authService = authService;
            }
            public async Task<BaseResponseDTO<AuthResponseDTO>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
            {
                var newUser = new User
                {
                    Slug = StringHelper.GenerateSlug(request.Username),
                    Username = request.Username,
                    Email = request.Email,
                    Password = _authService.HashPassword(request.Password),
                };

                var newMemberLocation = new Location
                {
                    Slug = StringHelper.GenerateSlug($"{request.Address} {request.District} {request.City}"),
                    User = newUser,
                    Address = request.Address,
                    City = request.City,
                    District = request.District,
                };

                await _dbContext.Users.AddAsync(newUser, cancellationToken);
                await _dbContext.Locations.AddAsync(newMemberLocation, cancellationToken);

                var refreshToken = _authService.GenerateRefreshToken(newUser);
                await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                return new BaseResponseDTO<AuthResponseDTO>
                {
                    Code = HttpStatusCode.OK,
                    Message = _localizer.Translate("successfully.register", new List<string> { $"{newUser.Slug}" }),
                    Data = new AuthResponseDTO
                    {
                        AccessToken = _authService.GenerateToken(newUser),
                        RefreshToken = refreshToken.RefreshTokenString,
                    }
                };
            }
        }
    }
}

