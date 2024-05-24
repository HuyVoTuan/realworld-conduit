using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Helpers;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Commands
{
    public class UpdateUserDetailCommand : IRequestWithBaseResponse<MinimalUserDTO>
    {
        public string AvatarUrl { get; init; }
        public string Email { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Bio { get; init; }
    };

    public class UpdateUserDetailCommandValidator : AbstractValidator<UpdateUserDetailCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<UpdateUserDetailCommandValidator> _localizer;

        public UpdateUserDetailCommandValidator(ApplicationDbContext dbContext, IStringLocalizer<UpdateUserDetailCommandValidator> localizer)
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
        }
    }

    internal class UpdateUserDetailCommandHandler : IRequestWithBaseResponseHandler<UpdateUserDetailCommand, MinimalUserDTO>
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<UpdateUserDetailCommandHandler> _localizer;

        public UpdateUserDetailCommandHandler(
        IAuthService authService,
        ApplicationDbContext dbContext,
        ICurrentUserService currentUserService,
        IStringLocalizer<UpdateUserDetailCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _authService = authService;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<MinimalUserDTO>> Handle(UpdateUserDetailCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.Id, cancellationToken);


            // Prevent re-generated slug if username stay the same
            var slug = StringHelper.GenerateSlug(request.Username);

            if (!StringHelper.IsSlugContainFullname(slug, existingUser.Slug))
            {
                existingUser.Slug = slug;
            }

            // Prevent re-hashed password if password stay the same => save performance
            if (!_authService.VerifyPassword(request.Password, existingUser.Password))
            {
                throw new RestfulAPIException(HttpStatusCode.Conflict, _localizer.Translate("cant_be_the_same", new List<string> { _localizer.Translate("password") }));
            }

            existingUser.Bio = request.Bio;
            existingUser.Email = request.Email;
            existingUser.Username = request.Username;
            existingUser.AvatarUrl = request.AvatarUrl;
            existingUser.Password = _authService.HashPassword(request.Password);

            // Save to database
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Map to DTO
            var userDTO = new MinimalUserDTO
            {
                Slug = existingUser.Slug,
                AvatarUrl = existingUser.AvatarUrl,
                Username = existingUser.Username,
                Bio = existingUser.Bio,
                Email = existingUser.Email,
            };

            return new BaseResponse<MinimalUserDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.update", new List<String> { "user" }),
                Data = userDTO
            };
        }
    }
}
