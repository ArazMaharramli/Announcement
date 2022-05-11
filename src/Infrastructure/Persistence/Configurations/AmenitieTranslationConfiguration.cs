using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class AmenitieTranslationConfiguration : TranslationConfiguration<AmenitieTranslation>
    {
        public override void Configure(EntityTypeBuilder<AmenitieTranslation> builder)
        {
            builder.Property(x => x.Name).IsRequired();

            base.Configure(builder);
        }
    }
}