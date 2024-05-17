using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class Tag : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public virtual ICollection<BlogTag> BlogTags { get; set; }
    }
}
