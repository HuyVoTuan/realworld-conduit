using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit_Domain.Entities;
using RealWorldConduit_Infrastructure.Constants;

namespace RealWorldConduit_Infrastructure.EntityConfigs
{
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable(nameof(Country), DatabaseSchema.UserSchema);

            builder.HasKey(x => x.Code);
        }
    }
}
