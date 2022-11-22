using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class ManagerClaimConfiguration : EntityConfiguration<ManagerClaim>
    {
        public override void Configure(EntityTypeBuilder<ManagerClaim> builder)
        {
            builder.Property(x => x.ClaimName).IsRequired();
        }
    }
}
