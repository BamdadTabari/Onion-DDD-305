﻿using _305.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _305.Infrastructure.EntityConfiguration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.id);

        builder.HasIndex(b => b.name).IsUnique();
        builder.Property(x => x.slug).IsRequired().HasMaxLength(1000);
        builder.HasIndex(x => x.slug).IsUnique();
        builder.Property(b => b.mobile)
            .IsRequired();

        #region Navigations

        builder
            .HasMany(x => x.user_roles)
            .WithOne(x => x.user)
            .HasForeignKey(x => x.userid)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
    }
}
