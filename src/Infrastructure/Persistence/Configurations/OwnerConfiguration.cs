using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations;

public class OwnerConfiguration : EntityConfiguration<Owner>
{
    public override void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasMany(x => x.Rooms)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId);

        base.Configure(builder);
    }
}