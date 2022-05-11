using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class RoomTypeTranslationConfiguration : TranslationConfiguration<RoomTypeTranslation>
    {
        public override void Configure(EntityTypeBuilder<RoomTypeTranslation> builder)
        {
            builder.Property(x => x.Name).IsRequired();

            base.Configure(builder);
        }
    }
}