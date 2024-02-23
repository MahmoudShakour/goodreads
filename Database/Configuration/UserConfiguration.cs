using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodreads.Database.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .HasMany(u=>u.Comments)
                .WithOne()
                .HasForeignKey(c=>c.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}