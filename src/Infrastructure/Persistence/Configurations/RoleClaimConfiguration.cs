using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class RoleClaimConfiguration : EntityConfiguration<RoleClaim>
    {
        public override void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.Property(x => x.ClaimName).IsRequired();
        }
    }
}
