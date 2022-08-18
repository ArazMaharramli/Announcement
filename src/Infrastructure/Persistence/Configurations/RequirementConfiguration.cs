using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class RequirementConfiguration : EntityConfiguration<Requirement>
    {
        public override void Configure(EntityTypeBuilder<Requirement> builder)
        {
            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Requirement);

            base.Configure(builder);
        }
    }
}