using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class Comment : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public Guid ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }
    }
}
