using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Constants;

namespace RealWorldConduit_Infrastructure.EntityConfigs
{
    internal class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(nameof(Comment), DatabaseSchema.BlogSchema);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content).IsRequired();

            builder.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(c => c.UserId)
                   .IsRequired();

            builder.HasOne(c => c.Blog)
                   .WithMany(b => b.Comments)
                   .HasForeignKey(c => c.BlogId);

            builder.HasOne(c => c.ParentComment)
                   .WithMany()
                   .HasForeignKey(c => c.ParentCommentId);
        }
    }
}
