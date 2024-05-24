using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Domain.Constants;
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
    public class UpsertUserDetailAndLocationCommand : IRequestWithBaseResponse<UserDTO>
    {
        public string AvatarUrl { get; init; }
        public string Email { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Bio { get; init; }
        public string Address { get; init; }
        public string District { get; init; }
        public string Ward { get; init; }
        public string City { get; init; }
        public string CountryCode { get; init; }
    };

    public class UpsertUserDetailAndLocationCommandValidator : AbstractValidator<UpsertUserDetailAndLocationCommand>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<UpsertUserDetailAndLocationCommandValidator> _localizer;

        public UpsertUserDetailAndLocationCommandValidator(ApplicationDbContext dbContext, IStringLocalizer<UpsertUserDetailAndLocationCommandValidator> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;

            RuleFor(x => x.Username).NotEmpty()
                                   .OverridePropertyName(_localizer.Translate("username"))
                                   .WithMessage(_localizer.Translate("cannot_be_empty"));


            RuleFor(x => x.Password).NotEmpty()
                                    .OverridePropertyName(_localizer.Translate("password"))
                                    .WithMessage(_localizer.Translate("failure.cant_be_empty"))
                                    .MinimumLength(6)
                                    .OverridePropertyName(_localizer.Translate("password"))
                                    .WithMessage(_localizer.Translate("validation_rules.min_6_length"));

            RuleFor(x => x.Email).NotEmpty()
                                 .OverridePropertyName(_localizer.Translate("email"))
                                 .WithMessage(_localizer.Translate("cannot_be_empty"))
                                 .EmailAddress()
                                 .OverridePropertyName(_localizer.Translate("email"))
                                 .WithMessage(_localizer.Translate("invalid"));

            RuleFor(x => x.Address).NotEmpty()
                                   .OverridePropertyName(_localizer.Translate("address"))
                                   .WithMessage(_localizer.Translate("failure.cant_be_empty"));

            RuleFor(x => x.District).NotEmpty()
                                    .OverridePropertyName(_localizer.Translate("district"))
                                    .WithMessage(_localizer.Translate("failure.cant_be_empty"));

            RuleFor(x => x.Ward).NotEmpty()
                                .OverridePropertyName(_localizer.Translate("ward"))
                                .WithMessage(_localizer.Translate("failure.cant_be_empty"));

            RuleFor(x => x.City).NotEmpty()
                                .OverridePropertyName(_localizer.Translate("city"))
                                .WithMessage(_localizer.Translate("failure.cant_be_empty"))
                                .Must(StringHelper.IsValidString)
                                .OverridePropertyName(_localizer.Translate("city"))
                                .WithMessage(_localizer.Translate("failure.invalid"));

            // TODO: Implement Proper Validation
            RuleFor(x => x.CountryCode).NotEmpty()
                                       .OverridePropertyName(_localizer.Translate("country"))
                                       .WithMessage(_localizer.Translate("cant_be_empty"))
                                       .Must(code =>
                                       {
                                           return code == CountryCode.vietnamCode || code == CountryCode.usaCode;
                                       })
                                       .OverridePropertyName(_localizer.Translate("country"))
                                       .WithMessage(_localizer.Translate("not_supported"));
        }
    }

    internal class UpsertUserDetailAndLocationCommandHandler : IRequestWithBaseResponseHandler<UpsertUserDetailAndLocationCommand, UserDTO>
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<UpsertUserDetailAndLocationCommandHandler> _localizer;

        public UpsertUserDetailAndLocationCommandHandler(
        IAuthService authService,
        ApplicationDbContext dbContext,
        ICurrentUserService currentUserService,
        IStringLocalizer<UpsertUserDetailAndLocationCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _authService = authService;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<UserDTO>> Handle(UpsertUserDetailAndLocationCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.Include(x => x.Location).FirstOrDefaultAsync(x => x.Id == _currentUserService.Id, cancellationToken);


            // Prevent re-generated slug if username stay the same
            var slug = StringHelper.GenerateSlug(request.Username);
            if (!StringHelper.IsSlugContainFullname(slug, existingUser.Slug))
            {
                existingUser.Slug = slug;
            }

            // Prevent re-hashed password if password stay the same => save performance
            if (!_authService.VerifyPassword(request.Password, existingUser.Password))
            {
                existingUser.Password = _authService.HashPassword(request.Password);
            }

            if (request.Email != existingUser.Email)
            {
                if (await _dbContext.Users.AnyAsync(x => x.Email == request.Email))
                {
                    throw new RestfulAPIException(HttpStatusCode.Conflict, _localizer.Translate("existed"));
                }
                else
                {
                    existingUser.Email = request.Email;
                }
            }

            existingUser.Bio = request.Bio;
            existingUser.Email = request.Email;
            existingUser.Username = request.Username;
            existingUser.AvatarUrl = request.AvatarUrl;
            existingUser.Location.Address = request.Address;
            existingUser.Location.District = request.District;
            existingUser.Location.Ward = request.Ward;
            existingUser.Location.City = request.City;
            existingUser.Location.CountryCode = request.CountryCode;


            // Save to database
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Map to DTO
            var userDTO = new UserDTO
            {
                Slug = existingUser.Slug,
                AvatarUrl = existingUser.AvatarUrl,
                Username = existingUser.Username,
                Bio = existingUser.Bio,
                Email = existingUser.Email,
                Locations = new LocationDTO
                {
                    Slug = existingUser.Location.Slug,
                    Address = existingUser.Location.Address,
                    District = existingUser.Location.District,
                    Ward = existingUser.Location.Ward,
                    City = existingUser.Location.City,
                }
            };

            return new BaseResponse<UserDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.update", new List<String> { "user" }),
                Data = userDTO
            };
        }
    }
}
