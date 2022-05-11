using System;
using Infrastructure.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Identity.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.RegisteredAt).HasDefaultValueSql("getdate()");
            builder.HasMany(x => x.RefreshTokens)
                .WithOne(x => x.User);
        }
    }
}
