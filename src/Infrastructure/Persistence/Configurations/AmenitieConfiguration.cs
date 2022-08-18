using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class AmenitieConfiguration : EntityConfiguration<Amenitie>
    {
        public override void Configure(EntityTypeBuilder<Amenitie> builder)
        {
            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Amenitie);

            base.Configure(builder);
        }
    }
}