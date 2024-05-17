using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Constants;

namespace RealWorldConduit_Infrastructure.EntityConfigs
{
    internal class BlogConfig : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable(nameof(Blog), DatabaseSchema.BlogSchema);

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Slug).IsUnique();

            builder.Property(x => x.Slug).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Content).IsRequired();

            builder.HasOne(b => b.Author)
                   .WithMany(aut => aut.Blogs)
                   .HasForeignKey(b => b.AuthorId);
        }
    }
}
