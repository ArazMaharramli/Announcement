using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class CategoryTranslationConfiguration : TranslationConfiguration<CategoryTranslation>
    {
        public override void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.HasIndex(x => x.Slug).IsUnique();
            builder.Property(x => x.Name).IsRequired();

            builder.OwnsOne(x => x.Meta, m =>
            {
                m.Property(x => x.Title).IsRequired();
            });

            base.Configure(builder);
        }
    }
}