using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class RoomConfiguration : EntityConfiguration<Room>
    {
        public override void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(x => x.Name).IsRequired();

            builder.HasIndex(x => x.Slug).IsUnique();

            builder.OwnsOne(x => x.Meta).WithOwner();

            builder.OwnsOne(x => x.Address).WithOwner();

            builder.OwnsOne(x => x.Contact).WithOwner();

            builder.OwnsMany(x => x.Medias, m =>
            {
                m.WithOwner().HasForeignKey(x => x.ContentId);
                m.HasKey(x => x.Id);
            });

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Rooms);


            builder.HasMany(x => x.Amenities)
                .WithMany(x => x.Rooms);

            builder.HasMany(x => x.Requirements)
                .WithMany(x => x.Rooms);

            base.Configure(builder);
        }
    }
}