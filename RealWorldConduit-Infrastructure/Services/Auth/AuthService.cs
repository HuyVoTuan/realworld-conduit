using RealWorldConduit_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldConduit_Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        public RefreshToken GenerateRefreshToken(User user)
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(User user)
        {
            throw new NotImplementedException();
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
