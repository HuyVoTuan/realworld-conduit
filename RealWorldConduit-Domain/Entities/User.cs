﻿using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string Slug { get; set; }
        public string AvatarUrl { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<FavoriteBlog> FavoriteBlogs { get; set; }
        public virtual ICollection<Friendship> BeingFollowedUser { get; set; }
        public virtual ICollection<Friendship> Follower { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}