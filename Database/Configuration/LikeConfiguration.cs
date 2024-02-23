using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using goodreads.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace goodreads.Database.Configuration
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder
                .HasKey(l=>l.Id);
            
            builder
                .Property(l=>l.AppUserId)
                .IsRequired();
            
            builder
                .Property(l=>l.ReviewId)
                .IsRequired();
            
            builder
                .Property(l=>l.CreatedAt)
                .IsRequired();
            
            builder
                .HasIndex(l=>new{l.AppUserId,l.ReviewId})
                .IsUnique(true);
            

        }
    }
}