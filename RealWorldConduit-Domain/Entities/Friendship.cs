using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class Friendship : BaseEntity
    {
        public Guid BeingFollowedUserId { get; set; }
        public virtual User BeingFollowedUser { get; set; }
        public Guid FollowerId { get; set; }
        public virtual User Follower { get; set; }
    }
}
