using Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Base
{
    public abstract class TranslationConfiguration<TBase> : EntityConfiguration<TBase>
    where TBase : Translation
    {
        public override void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.Property(x => x.LangCode).IsRequired();

            base.Configure(builder);
        }
    }
}
