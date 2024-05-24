using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Constants;

namespace RealWorldConduit_Infrastructure.EntityConfigs
{
    internal class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable(nameof(Location), DatabaseSchema.UserSchema);

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Address);
            builder.HasIndex(x => x.Slug).IsUnique();

            builder.Property(x => x.Slug).IsRequired();
            builder.Property(x => x.City).IsRequired();
            builder.Property(x => x.Address).IsRequired();

            builder.HasOne(l => l.Country)
                   .WithMany(c => c.Locations)
                   .HasForeignKey(l => l.CountryCode);
        }
    }
}
