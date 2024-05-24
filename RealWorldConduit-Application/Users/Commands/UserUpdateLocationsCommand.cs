using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RealWorldConduit_Application.Users.DTOs;
using RealWorldConduit_Domain.Constants;
using RealWorldConduit_Infrastructure;
using RealWorldConduit_Infrastructure.Commons;
using RealWorldConduit_Infrastructure.Commons.Base;
using RealWorldConduit_Infrastructure.Extensions;
using RealWorldConduit_Infrastructure.Helpers;
using RealWorldConduit_Infrastructure.Services.Auth;
using System.Net;

namespace RealWorldConduit_Application.Users.Commands
{
    public class UserUpdateLocationsCommandDTO
    {
        public string Address { get; init; }
        public string District { get; init; }
        public string Ward { get; init; }
        public string City { get; init; }
        public string CountryCode { get; init; }
    };

    public class UserUpdateLocationsCommandDTOValidator : AbstractValidator<UserUpdateLocationsCommandDTO>
    {
        private readonly IStringLocalizer<UserUpdateLocationsCommandDTOValidator> _localizer;

        public UserUpdateLocationsCommandDTOValidator(IStringLocalizer<UserUpdateLocationsCommandDTOValidator> localizer)
        {
            _localizer = localizer;

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

    public class UserUpdateLocationsCommand : IRequestWithBaseResponse<LocationDTO>
    {
        public string Slug { get; set; }
        public string Address { get; init; }
        public string District { get; init; }
        public string Ward { get; init; }
        public string City { get; init; }
        public string CountryCode { get; init; }
    };

    internal class UserUpdateLocationsCommandHandler : IRequestWithBaseResponseHandler<UserUpdateLocationsCommand, LocationDTO>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer<UserRefreshTokenCommandHandler> _localizer;

        public UserUpdateLocationsCommandHandler(
        ApplicationDbContext dbContext,
        ICurrentUserService currentUser,
        IStringLocalizer<UserRefreshTokenCommandHandler> localizer)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _currentUser = currentUser;
        }
        public async Task<BaseResponse<LocationDTO>> Handle(UserUpdateLocationsCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.Include(x => x.Location)
                                                     .FirstOrDefaultAsync(x => x.Id == _currentUser.Id && x.Location.Slug == request.Slug, cancellationToken);

            if (existingUser is null)
            {
                throw new RestfulAPIException(HttpStatusCode.NotFound, _localizer.Translate("not_found"));
            }

            // Prevent re-generated slug if username stay the same
            var slug = StringHelper.GenerateSlug($"{request.Address} {request.City}");
            if (!StringHelper.IsSlugContainFullname(slug, existingUser.Location.Slug))
            {
                existingUser.Location.Slug = slug;
            }

            existingUser.Location.Address = request.Address;
            existingUser.Location.District = request.District;
            existingUser.Location.Ward = request.Ward;
            existingUser.Location.City = request.City;
            existingUser.Location.CountryCode = request.CountryCode;

            // Save to database
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Map to DTO
            var locationDTO = new LocationDTO
            {
                Slug = existingUser.Location.Slug,
                Address = existingUser.Location.Address,
                District = existingUser.Location.District,
                Ward = existingUser.Location.Ward,
                City = existingUser.Location.City,
                CountryCode = existingUser.Location.CountryCode,
            };

            return new BaseResponse<LocationDTO>
            {
                Code = HttpStatusCode.OK,
                Message = _localizer.Translate("successful.update", new List<String> { "user" }),
                Data = locationDTO
            };
        }
    }
}
