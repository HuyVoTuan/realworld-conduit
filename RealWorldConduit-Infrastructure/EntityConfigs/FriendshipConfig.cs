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

            builder.HasKey(x => new { x.UserThatFollowId, x.UserBeingFollowedId });

            builder.HasOne(f => f.UserThatFollow)
                   .WithMany(fl => fl.UserThatFollow)
                   .HasForeignKey(f => f.UserThatFollowId);

            builder.HasOne(f => f.UserBeingFollowed)
                   .WithMany(fl => fl.UserBeingFollowed)
                   .HasForeignKey(f => f.UserBeingFollowedId);
        }
    }
}
