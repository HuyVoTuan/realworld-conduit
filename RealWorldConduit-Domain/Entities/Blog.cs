using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class Blog : BaseEntity<Guid>
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; }
        public virtual ICollection<BlogTag> BlogTags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FavoriteBlog> FavoriteBlogs { get; set; }
    }
}
