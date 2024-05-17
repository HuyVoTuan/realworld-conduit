using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class BlogTag : BaseEntity
    {
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
