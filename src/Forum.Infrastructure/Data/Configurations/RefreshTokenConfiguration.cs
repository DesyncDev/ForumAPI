using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Domain.Commom.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.HasKey(x => x.Id);

            builder.Property(t => t.Token)
                .IsRequired();

            // Index
            builder.HasIndex(t => t.Token)
                .IsUnique();

            // Ignores
            builder.Ignore(ie => ie.IsExpired);
            builder.Ignore(ia => ia.IsActive);

            // Relations
            builder.HasOne(u => u.User)
                .WithMany(rt => rt.RefreshTokens)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}