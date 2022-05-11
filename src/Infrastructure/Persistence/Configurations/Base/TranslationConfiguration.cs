using System;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Base
{
    public abstract class TranslationConfiguration<TBase> : IEntityTypeConfiguration<TBase>
    where TBase : Translation
    {
        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.Property(x => x.LangCode).IsRequired();
        }
    }
}
