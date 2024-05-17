using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Constants;

namespace RealWorldConduit_Infrastructure.EntityConfigs
{
    internal class FriendshipConfig : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable(nameof(Friendship), DatabaseSchema.UserSchema);

            builder.HasKey(x => new { x.BeingFollowedUserId, x.FollowerId });

            builder.HasOne(f => f.Follower)
                   .WithMany(fl => fl.Follower)
                   .HasForeignKey(f => f.FollowerId);

            builder.HasOne(f => f.BeingFollowedUser)
                   .WithMany(fl => fl.BeingFollowedUser)
                   .HasForeignKey(f => f.BeingFollowedUserId);
        }
    }
}
