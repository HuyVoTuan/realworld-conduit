using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class RefreshToken : BaseEntity<Guid>
    {
        public string RefreshTokenString { get; set; }
        public DateTime ExpiredTime { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
