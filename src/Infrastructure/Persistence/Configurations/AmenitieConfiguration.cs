using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class AmenitieConfiguration : IEntityTypeConfiguration<Amenitie>
    {
        public void Configure(EntityTypeBuilder<Amenitie> builder)
        {
            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Amenitie);
        }
    }
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasMany(x => x.Translations)
                .WithOne(x => x.RoomType);
        }
    }
}