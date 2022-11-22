using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Configurations.Base;

namespace Persistence.Configurations
{
    public class ManagerConfiguration : EntityConfiguration<Manager>
    {
        public override void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Surname).IsRequired();
            builder.Property(x => x.Email).IsRequired();

            builder.HasMany(x => x.Claims)
                .WithOne(x => x.Manager);

            builder.HasMany(x => x.Roles)
                .WithMany(x => x.Managers);


        }
    }
}
