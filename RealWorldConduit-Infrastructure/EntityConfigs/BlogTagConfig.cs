using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Constants;

namespace RealWorldConduit_Infrastructure.EntityConfigs
{
    internal class BlogTagConfig : IEntityTypeConfiguration<BlogTag>
    {
        public void Configure(EntityTypeBuilder<BlogTag> builder)
        {
            builder.ToTable(nameof(BlogTag), DatabaseSchema.BlogSchema);

            builder.HasKey(bl => new { bl.BlogId, bl.TagId });

            builder.HasOne(bl => bl.Blog)
                   .WithMany(b => b.BlogTags)
                   .HasForeignKey(bl => bl.BlogId);


            builder.HasOne(bl => bl.Tag)
                   .WithMany(t => t.BlogTags)
                   .HasForeignKey(bl => bl.TagId);
        }
    }
}
