using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations.Base
{
    public abstract class EntityConfiguration<TBase> : IEntityTypeConfiguration<TBase>
    where TBase : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.Ignore(x => x.DomainEvents);
        }
    }
}
