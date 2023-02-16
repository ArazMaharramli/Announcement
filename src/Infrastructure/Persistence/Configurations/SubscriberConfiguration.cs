using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations;

public class SubscriberConfiguration : EntityConfiguration<Subscriber>
{
    public override void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();
    }
}