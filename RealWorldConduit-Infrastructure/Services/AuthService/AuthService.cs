using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealWorldConduit_Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        private const int ACCESS_TOKEN_LIFE_TIME = 5;
        private const int REFRESH_TOKEN_LIFE_TIME = ACCESS_TOKEN_LIFE_TIME * 12;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public RefreshToken GenerateRefreshToken(User user)
        {
            string uniqueResfreshToken = StringHelper.GenerateRefreshToken();

            return new RefreshToken
            {
                UserId = user.Id,
                RefreshTokenString = uniqueResfreshToken,
                ExpiredTime = DateTime.UtcNow.AddMinutes(REFRESH_TOKEN_LIFE_TIME),
            };
        }

        public string GenerateToken(User user)
        {
            var credential = _configuration["AppCredential"];
            var key = Encoding.UTF8.GetBytes(credential);

            // Create a token structure base
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, $"{user.Slug}"),
                    new Claim(ClaimTypes.NameIdentifier, $"{user.Id}")
                }),
                Expires = DateTime.UtcNow.AddMinutes(ACCESS_TOKEN_LIFE_TIME),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };

            // Token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create token and write to string
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, 12);
        }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
