using RealWorldConduit_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
