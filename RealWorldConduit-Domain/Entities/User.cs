using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Slug { get; set; }
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public bool isActive { get; set; }
        public Guid LocationId { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FavoriteBlog> FavoriteBlogs { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<Friendship> UserThatFollow { get; set; }
        public virtual ICollection<Friendship> UserBeingFollowed { get; set; }
    }
}
