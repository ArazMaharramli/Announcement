using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations;

public class SearchConfiguration : EntityConfiguration<Search>
{
    public override void Configure(EntityTypeBuilder<Search> builder)
    {
        builder.HasOne(x => x.Category);

        builder.HasMany(x => x.Subscribers)
            .WithMany(x => x.Searches);
    }
}
