using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
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
            var slug = StringHelper.GenerateSlug(request.Username);

            var existingUser = await _dbContext.Users.Include(x => x.Locations).FirstOrDefaultAsync(x => x.Id == _currentUserService.Id, cancellationToken);


            // Prevent re-generated slug if username stay the same
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

            var existingUserLocations = existingUser.Locations.FirstOrDefault(x => x.Address == request.Address &&
                                                                                   x.District == request.District &&
                                                                                   x.Ward == request.Ward &&
                                                                                   x.City == request.City);

            if (existingUserLocations is null)
            {
                var newExistingUserLocation = new Location
                {
                    Slug = StringHelper.GenerateSlug($"{request.Address} {request.District} {request.City}"),
                    UserId = existingUser.Id,
                    Address = request.Address,
                    City = request.City,
                    District = request.District,
                    Ward = request.Ward,
                };

                await _dbContext.Locations.AddAsync(newExistingUserLocation, cancellationToken);
            }

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
                Locations = existingUser.Locations.Select(x => new LocationDTO
                {
                    Slug = x.Slug,
                    Address = x.Address,
                    District = x.District,
                    Ward = x.Ward,
                    City = x.City,
                })
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
