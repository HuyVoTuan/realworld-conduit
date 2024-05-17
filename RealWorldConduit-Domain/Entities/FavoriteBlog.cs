using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class FavoriteBlog : BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }
}
