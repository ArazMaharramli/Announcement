using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class CategoryConfiguration : EntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(x => x.Translations)
                .WithOne(x => x.Category);

            base.Configure(builder);
        }
    }
}