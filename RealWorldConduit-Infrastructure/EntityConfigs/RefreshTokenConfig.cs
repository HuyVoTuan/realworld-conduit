using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Constants;

namespace RealWorldConduit_Infrastructure.EntityConfigs
{
    internal class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(nameof(RefreshToken), DatabaseSchema.UserSchema);

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.RefreshTokenString).IsUnique();

            builder.Property(x => x.RefreshTokenString).IsRequired();
            builder.Property(x => x.ExpiredTime).IsRequired();

            builder.HasOne(rf => rf.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rf => rf.UserId);
        }
    }
}
