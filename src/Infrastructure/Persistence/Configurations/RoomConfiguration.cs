using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(x => x.Name).IsRequired();

            builder.HasIndex(x => x.Slug).IsUnique();

            builder.OwnsOne(x => x.Meta, m =>
            {
                m.Property(x => x.Title).IsRequired();
            });

            builder.OwnsOne(x => x.Address, a =>
            {
                a.Property(b => b.Location).IsRequired();
            });

            builder.OwnsOne(x => x.Contact, c =>
            {
                c.Property(x => x.Name).IsRequired();
                c.Property(x => x.Phone).IsRequired();
            });

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
        }
    }
}