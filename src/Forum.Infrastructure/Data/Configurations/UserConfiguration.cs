using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(254);

            builder.Property(ph => ph.PasswordHash)
                .IsRequired();

            builder.Property(dn => dn.DisplayName)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(b => b.Bio)
                .HasMaxLength(200);

            builder.Property(r => r.Role)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(ib =>ib.IsBanned)
                .IsRequired();

            builder.Property(ca => ca.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            // Index
            builder.HasIndex(e => e.Email)
                .IsUnique();

            builder.HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}