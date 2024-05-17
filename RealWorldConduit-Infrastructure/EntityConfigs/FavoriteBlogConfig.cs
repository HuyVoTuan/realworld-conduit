using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Constants;

namespace RealWorldConduit_Infrastructure.EntityConfigs
{
    internal class FavoriteBlogConfig : IEntityTypeConfiguration<FavoriteBlog>
    {
        public void Configure(EntityTypeBuilder<FavoriteBlog> builder)
        {
            builder.ToTable(nameof(FavoriteBlog), DatabaseSchema.BlogSchema);

            builder.HasKey(fb => new { fb.UserId, fb.BlogId });

            builder.HasOne(fb => fb.User)
                   .WithMany(u => u.FavoriteBlogs)
                   .HasForeignKey(fb => fb.UserId);

            builder.HasOne(fb => fb.Blog)
                  .WithMany(b => b.FavoriteBlogs)
                  .HasForeignKey(fb => fb.BlogId);
        }
    }
}
