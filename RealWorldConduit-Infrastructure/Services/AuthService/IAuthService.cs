using RealWorldConduit_Domain.Entities;

namespace RealWorldConduit_Infrastructure.Services.Auth
{
    public interface IAuthService
    {
        public string HashPassword(string plainPassword);
        public bool VerifyPassword(string plainPassword, string hashedPassword);
        public string GenerateToken(User user);
        public RefreshToken GenerateRefreshToken(User user);
    }
}
