using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class Friendship : BaseEntity
    {
        public Guid UserBeingFollowedId { get; set; }
        public virtual User UserBeingFollowed { get; set; }
        public Guid UserThatFollowId { get; set; }
        public virtual User UserThatFollow { get; set; }
    }
}
