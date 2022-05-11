using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class RequirementTranslationConfiguration : TranslationConfiguration<RequirementTranslation>
    {
        public override void Configure(EntityTypeBuilder<RequirementTranslation> builder)
        {
            builder.Property(x => x.Name).IsRequired();

            base.Configure(builder);
        }
    }
}